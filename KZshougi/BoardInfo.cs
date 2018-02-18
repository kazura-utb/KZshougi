using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KZshougi
{
    public class BoardInfo
    {

        public const int KOMA_NUM = 32;
        public const int BOARD_SIZE = 9;

        Bitmap[] komaPicArray = new Bitmap[KOMA_NUM];

        private Koma[] m_komaArray;

        private Koma NO;
        private Koma OU;
        private Koma HI;
        private Koma KA;
        private Koma KI1;
        private Koma GI1;
        private Koma KE1;
        private Koma KY1;
        private Koma KI2;
        private Koma GI2;
        private Koma KE2;
        private Koma KY2;
        private Koma FU1;
        private Koma FU2;
        private Koma FU3;
        private Koma FU4;
        private Koma FU5;
        private Koma FU6;
        private Koma FU7;
        private Koma FU8;
        private Koma FU9;
        private Koma GY;
        private Koma RY;
        private Koma UM;
        private Koma GIN1;
        private Koma KEN1;
        private Koma KYN1;
        private Koma GIN2;
        private Koma KEN2;
        private Koma KYN2;
        private Koma TO1;
        private Koma TO2;
        private Koma TO3;
        private Koma TO4;
        private Koma TO5;
        private Koma TO6;
        private Koma TO7;
        private Koma TO8;
        private Koma TO9;

        private Koma opHI;
        private Koma opKA;
        private Koma opKI1;
        private Koma opGI1;
        private Koma opKE1;
        private Koma opKY1;
        private Koma opKI2;
        private Koma opGI2;
        private Koma opKE2;
        private Koma opKY2;

        private Koma opFU1;
        private Koma opFU2;
        private Koma opFU3;
        private Koma opFU4;
        private Koma opFU5;
        private Koma opFU6;
        private Koma opFU7;
        private Koma opFU8;
        private Koma opFU9;

        private Koma opGY;
        private Koma opRY;
        private Koma opUM;
        private Koma opGIN1;
        private Koma opKEN1;
        private Koma opKYN1;
        private Koma opGIN2;
        private Koma opKEN2;
        private Koma opKYN2;
        private Koma opTO1;
        private Koma opTO2;
        private Koma opTO3;
        private Koma opTO4;
        private Koma opTO5;
        private Koma opTO6;
        private Koma opTO7;
        private Koma opTO8;
        private Koma opTO9;

        public BoardInfo()
        {
            NO = null;
            OU = new Koma();
            HI = new Koma();
            KA = new Koma();
            KI1 = new Koma();
            GI1 = new Koma();
            KE1 = new Koma();
            KY1 = new Koma();
            KI2 = new Koma();
            GI2 = new Koma();
            KE2 = new Koma();
            KY2 = new Koma();
            FU1 = new Koma();
            FU2 = new Koma();
            FU3 = new Koma();
            FU4 = new Koma();
            FU5 = new Koma();
            FU6 = new Koma();
            FU7 = new Koma();
            FU8 = new Koma();
            FU9 = new Koma();
            GY = new Koma();
            RY = new Koma();
            UM = new Koma();
            GIN1 = new Koma();
            KEN1 = new Koma();
            KYN1 = new Koma();
            GIN2 = new Koma();
            KEN2 = new Koma();
            KYN2 = new Koma();
            TO1 = new Koma();
            TO2 = new Koma();
            TO3 = new Koma();
            TO4 = new Koma();
            TO5 = new Koma();
            TO6 = new Koma();
            TO7 = new Koma();
            TO8 = new Koma();
            TO9 = new Koma();

            opHI = new Koma();
            opKA = new Koma();
            opKI1 = new Koma();
            opGI1 = new Koma();
            opKE1 = new Koma();
            opKY1 = new Koma();
            opKI2 = new Koma();
            opGI2 = new Koma();
            opKE2 = new Koma();
            opKY2 = new Koma();
            opFU1 = new Koma();
            opFU2 = new Koma();
            opFU3 = new Koma();
            opFU4 = new Koma();
            opFU5 = new Koma();
            opFU6 = new Koma();
            opFU7 = new Koma();
            opFU8 = new Koma();
            opFU9 = new Koma();
            opGY = new Koma();
            opRY = new Koma();
            opUM = new Koma();
            opGIN1 = new Koma();
            opKEN1 = new Koma();
            opKYN1 = new Koma();
            opGIN2 = new Koma();
            opKEN2 = new Koma();
            opKYN2 = new Koma();
            opTO1 = new Koma();
            opTO2 = new Koma();
            opTO3 = new Koma();
            opTO4 = new Koma();
            opTO5 = new Koma();
            opTO6 = new Koma();
            opTO7 = new Koma();
            opTO8 = new Koma();
            opTO9 = new Koma();

            OU.picIndex = 0;
            HI.picIndex = 1;
            KA.picIndex = 2;

            KI1.picIndex = 3;
            GI1.picIndex = 4;
            KE1.picIndex = 5;
            KY1.picIndex = 6;
            KI2.picIndex = 3;
            GI2.picIndex = 4;
            KE2.picIndex = 5;
            KY2.picIndex = 6;

            FU1.picIndex = 7;
            FU2.picIndex = 7;
            FU3.picIndex = 7;
            FU4.picIndex = 7;
            FU5.picIndex = 7;
            FU6.picIndex = 7;
            FU7.picIndex = 7;
            FU8.picIndex = 7;
            FU9.picIndex = 7;

            GY.picIndex = 8;
            RY.picIndex = 10;
            UM.picIndex = 11;

            GIN1.picIndex = 12;
            KEN1.picIndex = 13;
            KYN1.picIndex = 14;
            GIN2.picIndex = 12;
            KEN2.picIndex = 13;
            KYN2.picIndex = 14;
            
            TO1.picIndex = 15;

            opHI.picIndex = 17;
            opKA.picIndex = 18;

            opKI1.picIndex = 19;
            opGI1.picIndex = 20;
            opKE1.picIndex = 21;
            opKY1.picIndex = 22;
            opKI2.picIndex = 19;
            opGI2.picIndex = 20;
            opKE2.picIndex = 21;
            opKY2.picIndex = 22;

            opFU1.picIndex = 23;
            opFU2.picIndex = 23;
            opFU3.picIndex = 23;
            opFU4.picIndex = 23;
            opFU5.picIndex = 23;
            opFU6.picIndex = 23;
            opFU7.picIndex = 23;
            opFU8.picIndex = 23;
            opFU9.picIndex = 23;

            opGY.picIndex = 24;
            opRY.picIndex = 26;
            opUM.picIndex = 27;
            opGIN1.picIndex = 28;
            opKEN1.picIndex = 29;
            opKYN1.picIndex = 30;
            opGIN2.picIndex = 28;
            opKEN2.picIndex = 29;
            opKYN2.picIndex = 30;
            opTO1.picIndex = 31;

            m_komaArray = new Koma[81] 
            {
                opKY1,NO,  opFU1, NO, NO, NO, FU1, NO, KY1,
                opKE1,opHI,opFU2, NO, NO, NO, FU2, KA, KE1,
                opGI1,NO,  opFU3, NO, NO, NO, FU3, NO, GI1,
                opKI1,NO,  opFU4, NO, NO, NO, FU4, NO, KI1,
                opGY, NO,  opFU5, NO, NO, NO, FU5, NO, OU,
                opKI2,NO,  opFU6, NO, NO, NO, FU6, NO, KI2,
                opGI2,NO,  opFU7, NO, NO, NO, FU7, NO, GI2,
                opKE2,opKA,opFU8, NO, NO, NO, FU8, HI, KE2,
                opKY2,NO,  opFU9, NO, NO, NO, FU9, NO, KY2 
            };
        }


        public Koma getKomaInfo(int x, int y)
        {
            if (x < 0 || y < 0 || x >= BOARD_SIZE || y >= BOARD_SIZE) return null;
            return m_komaArray[x * 9 + y];
        }

        public int updateKoma(Koma m_dragDropKoma, int fromX, int fromY, int toX, int toY)
        {
            if (toX < 0 || toY < 0 || toX >= BOARD_SIZE || toY >= BOARD_SIZE) return -1;

            if (m_komaArray[toX * 9 + toY] == NO)
            {
                m_komaArray[toX * 9 + toY] = m_dragDropKoma;
                m_komaArray[fromX * 9 + fromY] = NO;
                return 0;
            }
            else if (toX == fromX && toY == fromY) return 0;
            else return -1;
        }
    }
}
