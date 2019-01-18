using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TetrisWPF.Properties;
using TetrisWPF;

public abstract class Tetromino
{
    public abstract int[,] shape { get; }
    public abstract String shapeName { get; }
    public abstract int rotated { get; set; }
    public abstract Color tetriminoColor { get; set; }
    public List<int[]> lokacja = new List<int[]>();

    public bool Stwórz()
    {
        for (int i = 0; i < shape.GetLength(0); i++)
        {
            for (int j = 0; j < shape.GetLength(1); j++)
            {
                if (shape[i, j] == 1)
                {
                    lokacja.Add(new int[] { i, (10 - shape.GetLength(1)) / 2 + j });
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (GameBoard.lokacjaOstatniegoTetrisaGrid[this.lokacja[i][0], this.lokacja[i][1]] == 1)
            {
                return false;
            }
        }

        Aktualizuj();
        return true;
    }
}


public class Tetromino_I : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[1, 4] { { 1, 1, 1, 1 } };
        }
    }

    public override String shapeName
    {
        get
        {
            return "I";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}

public class Tetromino_O : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[2, 2] { { 1, 1 }, { 1, 1 } };
        }
    }

    public override String shapeName
    {
        get
        {
            return "O";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}

public class Tetromino_T : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[2, 3] { { 0, 1, 0 }, { 1, 1, 1 } };
        }
    }

    public override String shapeName
    {
        get
        {
            return "T";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}

public class Tetromino_Z : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[2, 3] { { 1, 1, 0 }, { 0, 1, 1 } };
        }
    }

    public override String shapeName
    {
        get
        {
            return "Z";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}

public class Tetromino_S : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[2, 3] { { 0, 1, 1 }, { 1, 1, 0 } };
        }
    }

    public override String shapeName
    {
        get
        {
            return "S";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}

public class Tetromino_J : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[2, 3] { { 1, 0, 0 }, { 1, 1, 1 } };
        }
    }

    public override String shapeName
    {
        get
        {
            return "J";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}

public class Tetromino_L : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[2, 3] { { 0, 0, 1 }, { 1, 1, 1 } };
        }
    }

    public override String shapeName
    {
        get
        {
            return "L";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}

public class Tetromino_Single : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[1, 1] { { 1 } };
        }
    }

    public override String shapeName
    {
        get
        {
            return "o";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}