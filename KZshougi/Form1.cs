//! @file
//! アプリケーションフォーム
//****************************************************************************
//       (c) COPYRIGHT kazura_utb 2018-  All Rights Reserved.
//****************************************************************************
// FILE NAME     : Form1.cs
// PROGRAM NAME  : KZshougi
// FUNCTION      : フォーム
//
//****************************************************************************
//****************************************************************************
//
//****************************************************************************
//┌──┬─────┬──────────────────┬───────┐
//│履歴│   DATE   │              NOTES                 │     SIGN     │
//├──┼─────┼──────────────────┼───────┤
//│    │          │                                    │              │
//├──┼─────┼──────────────────┼───────┤
//│ A  │2018/02/17│新規作成                            │kazura_utb    │
//└──┴─────┴──────────────────┴───────┘
//****************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KZshougi
{
    public partial class Form1 : Form
    {

        private BufferedPanel panel1;
        private BufferedPanel panel_back;
        private BufferedPanel panel_board;

        //public CppWrapper cppWrapper;
        public bool loadResult;

        private Bitmap bkImg;
        private Bitmap whImg;

        //private BoardClass boardclass;
        //private CpuClass[] cpuClass;

        private int EVAL_THRESHOLD = 10000;

        private const int ON_NOTHING = 0;
        private const int ON_GAME = 1;
        private const int ON_EDIT = 3;
        private const int ON_HINT = 4;

        private const int TURN_HUMAN = 0;
        private const int TURN_CPU = 1;


        private const int COLOR_BLACK = 0;
        private const int COLOR_WHITE = 1;

        private const int INFINITY_SCORE = 2500000;

        private uint[] dcTable = { 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26 };

        private int m_mode = ON_NOTHING;
        private bool m_abort = false;

        private uint nowColor = COLOR_BLACK;
        //private Player nowPlayer;
        //private Player[] playerArray;

        private int m_passCount;

        private const ulong MOVE_PASS = 0;

        // ヒント表示用
        private uint m_hintLevel;
        private List<int[]> m_hintList;
        private int m_hintEvalMax;

        private Stopwatch m_sw;

        private GCHandle m_gcHandle_setCpuMessageDelegate;
        private GCHandle m_gcHandle_setPVLineDelegate;
        private GCHandle m_gcHandle_setMPCInfoDelegate;

        public delegate void SetMoveProperty(ulong moves);
        public delegate void SetNodeCountProperty(ulong nodeCount);
        public delegate void SetCpuMessageProperty(string cpuMsg);
        public delegate void SetMPCInfoProperty(string mpcMsg);

        public SetMoveProperty delegateObj;
        public SetNodeCountProperty nodeCountDelegate;
        public SetCpuMessageProperty cpuMessageDelegate;
        public SetCpuMessageProperty setPVLineDelegate;
        public SetCpuMessageProperty setMPCInfoDelegate;

        delegate void SetPVLineDelegate(string text);

        Font m_ft;
        Font m_ft2;
        Font m_ft3;
        Font m_ft4;

        public int m_event;

        private IntPtr cpuMessageDelegatePtr;
        private IntPtr setPVLineDelegatePtr;
        private IntPtr setMPCInfoDelegatePtr;

        private float m_scale;
        private float m_mass_size;
        private float m_fix_x = 0, m_fix_y = 0;
        private float m_board_width, m_board_height;
        private float m_board_start_x, m_board_start_y;
        private const float border_rate = (float)(255 / 2450.0);

        private Bitmap m_panel1_bitmap;
        private Bitmap m_back_bitmap;

        Bitmap[] komaPicArray;
        BoardInfo m_boardInfo;
        Koma m_dragDropKoma;
        int m_from_x;
        int m_from_y;
        int m_to_x;
        int m_to_y;
        float m_dragDrop_x;
        float m_dragDrop_y;

        public Form1()
        {
            InitializeComponent();

            m_boardInfo = new BoardInfo();
            m_dragDropKoma = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            m_back_bitmap = new System.Drawing.Bitmap(@"pic\japanese-chess-bg.jpg");
            m_panel1_bitmap = new System.Drawing.Bitmap(@"pic\japanese-chess-b02.jpg");


            // 駒の画像取得
            komaPicArray = new Bitmap[BoardInfo.KOMA_NUM];
            for (int i = 0; i < komaPicArray.Length; i++)
            {
                komaPicArray[i] = new System.Drawing.Bitmap(@"pic\koma\60x64\sgl" + (i + 1).ToString("D2") + ".png");
            }

            // 盤面背景のセット
            panel_back = new BufferedPanel(false);
            panel_back.Width = 800;
            panel_back.Height = 640;
            panel_back.Location = new Point(0, 0);
            panel_back.Paint += panel_back_Paint;
            panel_back.BackgroundImageLayout = ImageLayout.Zoom;
            panel_back.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.Controls.Add(panel_back);

            // 盤面のセット
            panel_board = new BufferedPanel(false);
            panel_board.Width = 480;
            panel_board.Height = 480;
            panel_board.Location = new Point(110, 60);
            panel_board.BackColor = Color.Transparent;
            panel_board.Paint += panel_board_Paint;
            panel_board.Resize += panel_Resize;
            panel_board.BackgroundImageLayout = ImageLayout.Zoom;
            panel_board.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            // 盤面背景の子として登録
            panel_back.Controls.Add(panel_board);

            // 盤面のセット
            panel1 = new BufferedPanel(true);
            panel1.Width = 480;
            panel1.Height = 480;
            panel1.Location = new Point(0, 0);
            panel1.BackColor = Color.Transparent;
            panel1.Paint += panel1_Paint;
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseUp += panel1_MouseUp;
            panel1.Resize += panel_Resize;
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            // 盤面背景の子として登録
            panel_board.Controls.Add(panel1);

            m_board_width = panel1.Width - (panel1.Width * border_rate);
            m_board_height = panel1.Height - (panel1.Height * border_rate);


            // 画像とフォントのスケーリング
            resizeObject(panel1);


        }


        void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Point pos;
            int num;
            MouseEventArgs mouseEvent = (MouseEventArgs)e;
            MouseButtons buttons = mouseEvent.Button;

            // 多分着手可能マスを表示すると思う。。
            m_from_x = (int)((mouseEvent.X - m_fix_x + 6) / m_mass_size);
            m_from_y = (int)((mouseEvent.Y - m_fix_y + 6) / m_mass_size);
            m_to_x = m_from_x;
            m_to_y = m_from_y;
            m_dragDropKoma = m_boardInfo.getKomaInfo(m_from_x, m_from_y);
        }

        void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouseEvent = (MouseEventArgs)e;
            if (e.Button == MouseButtons.Left) 
            {
                m_to_x = (int)((mouseEvent.X - m_fix_x + 6) / m_mass_size);
                m_to_y = (int)((mouseEvent.Y - m_fix_y + 6) / m_mass_size);

                m_dragDrop_x = m_to_x * m_mass_size + m_fix_x;
                m_dragDrop_y = m_to_y * m_mass_size + m_fix_y;

                panel1.Refresh();
            }
            
        }

        void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            int ret;

            ret = m_boardInfo.updateKoma(m_dragDropKoma, m_from_x, m_from_y, m_to_x, m_to_y);

            m_dragDropKoma = null;
            panel1.Refresh();

            if (ret == -1)
            {
                MessageBox.Show("そこには置けません。", "着手エラー",  MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private float stone_size_x, stone_size_y;
        private float last_move_fix_x, last_move_fix_y;
        private float canmove_x, canmove_y;
        private float font_scale_x, font_scale_y;
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Koma koma;
            for (int x = 0; x < BoardInfo.BOARD_SIZE; x++ )
            {
                for (int y = 0; y < BoardInfo.BOARD_SIZE; y++)
                {
                    koma = m_boardInfo.getKomaInfo(x, y);
                    // 駒の描画
                    if (koma != null)
                    {
                        if (koma != m_dragDropKoma) 
                        {
                            e.Graphics.DrawImage(
                                komaPicArray[koma.picIndex],
                                x * m_mass_size + m_fix_x,
                                y * m_mass_size + m_fix_y,
                                40 * m_scale,
                                40 * m_scale
                            );
                        }
                    }
                }
            }

            // Draw last
            if (m_dragDropKoma != null)
            {
                e.Graphics.DrawImage(
                    komaPicArray[m_dragDropKoma.picIndex],
                    m_dragDrop_x,
                    m_dragDrop_y,
                    40 * m_scale,
                    40 * m_scale
                );
            }

        }


        private void panel_back_Paint(object sender, PaintEventArgs e)
        {

            BufferedPanel bp = (BufferedPanel)sender;
            Rectangle board_rect1;

            if (panel_board.Width < panel_board.Height)
            {
                board_rect1 = new Rectangle((int)(panel_board.Location.X + m_board_start_x),
                    (int)(panel_board.Location.Y + m_board_start_y),
                    panel_board.Width, panel_board.Width);
            }
            else
            {
                board_rect1 = new Rectangle((int)(panel_board.Location.X + m_board_start_x),
                    (int)(panel_board.Location.Y + m_board_start_y),
                    panel_board.Height, panel_board.Height);
            }

            Rectangle board_rect2 = new Rectangle(bp.Location,
                new Size(bp.Width, bp.Height));

            Region rgn = new Region(board_rect2);
            rgn.Xor(board_rect1);

            e.Graphics.Clip = rgn;
            e.Graphics.DrawImage(m_back_bitmap, 0, 0, bp.Width, bp.Height);

        }


        private void panel_board_Paint(object sender, PaintEventArgs e)
        {
            BufferedPanel bp = (BufferedPanel)sender;

            if (bp.Width < bp.Height)
            {
                e.Graphics.DrawImage(m_panel1_bitmap, m_board_start_x, m_board_start_y, bp.Width, bp.Width);
            }
            else
            {
                e.Graphics.DrawImage(m_panel1_bitmap, m_board_start_x, m_board_start_y, bp.Height, bp.Height);
            }

        }

        // 盤面のリサイズ処理
        void panel_Resize(object sender, EventArgs e)
        {
            BufferedPanel pl = (BufferedPanel)sender;
            if (pl == panel1)
            {
                // 石やフォントのスケーリング処理
                resizeObject(pl);
            }
            else if (pl == panel_board)
            {
                // 盤面のスケーリング
                resizeScreen(pl);
            }
        }

        void resizeObject(BufferedPanel panel)
        {
            //label5.Text = "x=" + panel.Width + "y=" + panel.Height;
            float board_x, board_y;
            // 縦横の小さい方に合わせる
            if (panel.Width < panel.Height)
            {
                // margin考慮
                board_x = panel.Width - (panel.Width * border_rate);
                board_y = panel.Height - (panel.Width * border_rate);
                m_scale = board_x / (m_board_width + 16);
                m_mass_size = board_x / BoardInfo.BOARD_SIZE;
                m_fix_x = (float)30.5 * m_scale;
                m_fix_y = (board_y - board_x) / 2 + ((float)30.5 * m_scale);
            }
            else
            {
                board_x = panel.Width - (panel.Height * border_rate);
                board_y = panel.Height - (panel.Height * border_rate);
                m_scale = board_y / (m_board_height + 16);
                m_mass_size = board_y / BoardInfo.BOARD_SIZE;
                m_fix_x = (board_x - board_y) / 2 + ((float)30.5 * m_scale);
                m_fix_y = (float)30.5 * m_scale;
            }

            if (m_scale != 0)
            {
                // 各描画インスタンスの座標をスケーリング
                stone_size_x = (m_board_width / BoardInfo.BOARD_SIZE) * m_scale;
                stone_size_y = (m_board_height / BoardInfo.BOARD_SIZE) * m_scale;
                last_move_fix_x = (21 * m_scale) + m_fix_x;
                last_move_fix_y = (21 * m_scale) + m_fix_y;
                canmove_x = (19 * m_scale) + m_fix_x;
                canmove_y = (21 * m_scale) + m_fix_y;
                font_scale_x = (2 * m_scale) + m_fix_x;
                font_scale_y = (14 * m_scale) + m_fix_y;

                // フォントスケーリング
                m_ft = new Font("MS UI Gothic", 9 * m_scale);
                m_ft2 = new Font("MS UI Gothic", 8 * m_scale);
                m_ft3 = new Font("Arial", 18 * m_scale, FontStyle.Bold | FontStyle.Italic);
                m_ft4 = new Font("Arial", 15 * m_scale, FontStyle.Bold | FontStyle.Italic);
            }
        }


        void resizeScreen(BufferedPanel bp)
        {
            if (bp.Width < bp.Height)
            {
                m_board_start_x = 0;
                m_board_start_y = (bp.Height - bp.Width) / 2;
            }
            else
            {
                m_board_start_x = (bp.Width - bp.Height) / 2;
                m_board_start_y = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_mode = ON_GAME;
        }



    }



}
