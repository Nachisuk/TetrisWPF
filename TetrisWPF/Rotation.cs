using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWPF
{
    public abstract class RotateAbstract
    {
        public abstract void Rotate(ref int[,] kształt, ref List<int[]> lokacja, String typ, ref int stopien);
        public abstract void IsOutOfBounds(ref List<int[]> lokacja);
        public abstract Boolean IsColliding(List<int[]> lokacja);

        public int[,] ObrocKształt(int[,] kształt)
        {
            int[,] tmpKształt = new int[kształt.GetLength(1), kształt.GetLength(0)];
            //transportowanie kolejnych "wartosci" pod polami:
            for (int i = 0; i < kształt.GetLength(1); i++)
            {
                for (int j = 0; j < kształt.GetLength(0); j++)
                {
                    tmpKształt[i, j] = kształt[kształt.GetLength(0) - 1 - j, i];
                }
            }
            kształt = tmpKształt;
            return kształt;
        }

        public void RotateMethod(ref int[,] kształt, ref List<int[]> lokacja, String typ, ref int stopien)
        {
            //tworzenie backupu
            int[,] BU_kształt = kształt.Clone() as int[,];
            List<int[]> BU_lokacja = new List<int[]>();
            for (int i = 0; i < 4; i++)
                BU_lokacja.Add(lokacja[i].Clone() as int[]);
            int BU_stopienObrocenia = stopien;


            Rotate(ref kształt, ref lokacja, typ,ref stopien);
            IsOutOfBounds(ref lokacja);

            if(IsColliding(lokacja))
            {
                kształt = BU_kształt;
                lokacja = BU_lokacja;
                stopien = BU_stopienObrocenia;
            }
        }
    }

    public class RotateRight : RotateAbstract
    {
        public override void Rotate(ref int[,] kształt, ref List<int[]> lokacja, String typ, ref int stopienObrocenia)
        {
            switch (typ)
            {
                case "I":
                    //sprawdzamy który z stopien obrocenia jest 0 - lezy czy 1 - pionowy
                    switch (stopienObrocenia)
                    {
                        case 0:
                            //przekształcenie aby było w stopniu 1
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia++;
                            lokacja[3] = lokacja[1];
                            lokacja[0] = new int[] { lokacja[3][0] - 3, lokacja[3][1] };
                            lokacja[1] = new int[] { lokacja[3][0] - 2, lokacja[3][1] };
                            lokacja[2] = new int[] { lokacja[3][0] - 1, lokacja[3][1] };
                            break;

                        case 1:
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia = 0;
                            lokacja[1] = lokacja[3];
                            lokacja[0] = new int[] { lokacja[3][0], lokacja[3][1] - 1 };
                            lokacja[2] = new int[] { lokacja[3][0], lokacja[3][1] + 1 };
                            lokacja[3] = new int[] { lokacja[3][0], lokacja[3][1] + 2 };
                            break;
                    }
                    break;

                case "T":
                    switch (stopienObrocenia)
                    {
                        case 0:
                            //przekształcenie aby było w stopniu 1
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia++;
                            lokacja[3] = lokacja[2];
                            lokacja[0] = new int[] { lokacja[3][0] - 2, lokacja[3][1] };
                            lokacja[1] = new int[] { lokacja[3][0] - 1, lokacja[3][1] };
                            lokacja[2] = new int[] { lokacja[3][0] - 1, lokacja[3][1] + 1 };
                            break;

                        case 1:
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia++;
                            lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                            break;

                        case 2:
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia++;
                            lokacja[2] = lokacja[1];
                            lokacja[1] = lokacja[0];
                            lokacja[0] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                            break;

                        case 3:
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia = 0;
                            lokacja[0] = lokacja[2];
                            lokacja[2] = lokacja[3];
                            lokacja[1] = new int[] { lokacja[2][0], lokacja[2][1] - 1 };
                            lokacja[3] = new int[] { lokacja[2][0], lokacja[2][1] + 1 };
                            break;
                    }
                    break;

                case "O":
                    return;
                    break;

                case "S":
                    switch (stopienObrocenia)
                    {
                        case 0:
                            //przekształcenie aby było w stopniu 1
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia++;
                            lokacja[2] = lokacja[0];
                            lokacja[1] = new int[] { lokacja[2][0], lokacja[2][1] - 1 };
                            lokacja[0] = new int[] { lokacja[1][0] - 1, lokacja[1][1] };
                            break;

                        case 1:
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia = 0;
                            lokacja[0] = lokacja[2];
                            lokacja[2] = new int[] { lokacja[3][0], lokacja[3][1] - 1 };
                            lokacja[1] = new int[] { lokacja[0][0], lokacja[0][1] + 1 };
                            break;
                    }
                    break;

                case "Z":
                    switch (stopienObrocenia)
                    {
                        case 0:
                            //przekształcenie aby było w stopniu 1
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia++;
                            lokacja[3] = lokacja[2];
                            lokacja[2] = new int[] { lokacja[1][0], lokacja[1][1] + 1 };
                            lokacja[0] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                            break;

                        case 1:
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia = 0;
                            lokacja[2] = lokacja[3];
                            lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                            lokacja[3] = new int[] { lokacja[2][0], lokacja[2][1] + 1 };
                            break;
                    }
                    break;

                case "J":
                    switch (stopienObrocenia)
                    {
                        case 0:
                            //przekształcenie aby było w stopniu 1
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia++;
                            lokacja[3] = lokacja[2];
                            lokacja[2] = new int[] { lokacja[3][0] - 1, lokacja[3][1] };
                            lokacja[0] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                            lokacja[1] = new int[] { lokacja[0][0], lokacja[0][1] + 1 };
                            break;

                        case 1:
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia++;
                            lokacja[1] = lokacja[2];
                            lokacja[2] = new int[] { lokacja[1][0], lokacja[1][1] + 1 };
                            lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                            lokacja[3] = new int[] { lokacja[2][0] + 1, lokacja[2][1] };
                            break;

                        case 2:
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia++;
                            lokacja[0] = new int[] { lokacja[1][0] - 1, lokacja[1][1] };
                            lokacja[3] = new int[] { lokacja[1][0] + 1, lokacja[1][1] };
                            lokacja[2] = new int[] { lokacja[3][0], lokacja[3][1] - 1 };
                            break;

                        case 3:
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia = 0;
                            lokacja[1] = lokacja[2];
                            lokacja[2] = lokacja[3];
                            lokacja[0] = new int[] { lokacja[1][0] - 1, lokacja[1][1] };
                            lokacja[3] = new int[] { lokacja[2][0], lokacja[2][1] + 1 };
                            break;
                    }
                    break;

                case "L":
                    switch (stopienObrocenia)
                    {
                        case 0:
                            //przekształcenie aby było w stopniu 1
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia++;
                            lokacja[1] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                            lokacja[0] = new int[] { lokacja[1][0] - 1, lokacja[1][1] };
                            break;

                        case 1:
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia++;
                            lokacja[3] = new int[] { lokacja[2][0], lokacja[2][1] - 1 };
                            lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                            lokacja[2] = new int[] { lokacja[1][0], lokacja[1][1] + 1 };
                            break;

                        case 2:
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia++;
                            lokacja[1] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                            lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                            lokacja[3] = new int[] { lokacja[2][0] + 1, lokacja[2][1] };
                            break;

                        case 3:
                            kształt = ObrocKształt(kształt);
                            stopienObrocenia = 0;
                            lokacja[0] = lokacja[2];
                            lokacja[2] = new int[] { lokacja[3][0], lokacja[3][1] - 1 };
                            lokacja[1] = new int[] { lokacja[3][0], lokacja[3][1] - 2 };
                            break;
                    }
                    break;
            }
        }

        public override void IsOutOfBounds(ref List<int[]> lokacja)
        {
            bool poza = true;
            while (poza == true)
            {
                int i;
                for (i = 0; i < 4; i++)
                {
                    if (lokacja[i][1] < 0) //wychodzi za lewą granice
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            lokacja[j][0] += 0;
                            lokacja[j][1] += 1;
                        }
                    }
                    else if (lokacja[i][1] >= GameBoard.Instance.GameBoard1.ColumnDefinitions.Count)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            lokacja[j][0] += 0;
                            lokacja[j][1] -= 1;
                        }
                    }
                }
                if (i == 4)
                    poza = false;
            }
        }

        public override Boolean IsColliding(List<int[]> lokacja)
        {
            for (int i = 0; i < 4; i++)
            {
                if (lokacja[i][0] >= 0)
                    if (GameBoard.lokacjaOstatniegoTetrisaGrid[lokacja[i][0], lokacja[i][1]] == 1)
                    {
                        return true;
                    }
            }
            return false;
        }
    }

    public class RotateLeft : RotateAbstract
    {
        public override bool IsColliding(List<int[]> lokacja)
        {
            throw new NotImplementedException();
        }

        public override void IsOutOfBounds(ref List<int[]> lokacja)
        {
            throw new NotImplementedException();
        }

        public override void Rotate(ref int[,] kształt, ref List<int[]> lokacja, string typ, ref int stopien)
        {
            throw new NotImplementedException();
        }
    }
}
