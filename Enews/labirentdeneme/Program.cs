using System;

class LabirentOyunu
{
    static char[,] labirent;
    static int oyuncuX;
    static int oyuncuY;
    static int cikisX;
    static int cikisY;
    static bool labirentGizli = false;
    static Random random = new Random();
    static ConsoleColor currentBackgroundColor = ConsoleColor.Gray;
    static ConsoleColor[] renkPaleti = { ConsoleColor.DarkGreen, ConsoleColor.Black, ConsoleColor.DarkRed, ConsoleColor.DarkYellow };

    static void Main()
    {
        OyunuBaslat();
    }

    static void OyunuBaslat()
    {
        for (int level = 1; level <= 100; level++)
        {
            LabirentiHazirla(level);

            if (level % 33 == 1)
            {
                currentBackgroundColor = renkPaleti[(level / 11) % renkPaleti.Length];
            }

            Console.BackgroundColor = currentBackgroundColor;
            Console.Clear();

            LabirentiGoster();

            Console.WriteLine($"Level {level}");
            Console.WriteLine("Labirenti görmek için bir tuşa basın...");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.KeyChar == 'h')
            {
                labirentGizli = !labirentGizli;
                LabirentiGoster();
            }
            else if (keyInfo.KeyChar == 'r')
            {
                LabirentiHazirla(level);
                LabirentiGoster();
            }
            else if (keyInfo.KeyChar == 'i')
            {
                CikisIpuclariCiz();
                LabirentiGoster();
            }
            while (true)
            {
                keyInfo = Console.ReadKey(true);
                char hareketYonu = keyInfo.KeyChar;

                if (hareketYonu == 'h')
                {
                    labirentGizli = !labirentGizli;
                    LabirentiGoster();
                }
                else if (hareketYonu == 'r')
                {
                    LabirentiHazirla(level);
                    LabirentiGoster();
                }
                else if (hareketYonu == 'i')
                {
                    CikisIpuclariCiz();
                    LabirentiGoster();
                }
                else if (HareketKontrol(hareketYonu))
                {
                    Console.BackgroundColor = currentBackgroundColor;
                    Console.Clear();
                    LabirentiGoster();

                    if (oyuncuX == cikisX && oyuncuY == cikisY)
                    {
                        Console.WriteLine("Tebrikler! Çıkışa ulaştınız!");
                        break;
                    }

                    int mesafe = Math.Abs(oyuncuX - cikisX) + Math.Abs(oyuncuY - cikisY);
                    if (mesafe <= 3)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = ConsoleColor.Blue;

                    Console.WriteLine(mesafe <= 3 ? "Sıcak" : "Soğuk");
                    Console.ResetColor();
                }
            }
        }

        Console.WriteLine("Oyun tamamlandı!");
    }

    static void CikisIpuclariCiz()
    {
        int hedefX = cikisX;
        int hedefY = cikisY;

        while (oyuncuX != hedefX || oyuncuY != hedefY)
        {
            labirent[oyuncuY, oyuncuX] = '.';
            if (oyuncuX < hedefX) oyuncuX++;
            else if (oyuncuX > hedefX) oyuncuX--;

            if (oyuncuY < hedefY) oyuncuY++;
            else if (oyuncuY > hedefY) oyuncuY--;
        }
    }

    static void RastgeleBaslat()
    {
        do
        {
            oyuncuX = random.Next(1, labirent.GetLength(1) - 1);
            oyuncuY = random.Next(1, labirent.GetLength(0) - 1);
        } while (labirent[oyuncuY, oyuncuX] == '|');
    }

    static bool HareketKontrol(char yon)
    {
        int hedefX = oyuncuX;
        int hedefY = oyuncuY;

        switch (yon)
        {
            case 'w':
                hedefY--;
                break;
            case 's':
                hedefY++;
                break;
            case 'a':
                hedefX--;
                break;
            case 'd':
                hedefX++;
                break;
        }

        if (hedefX >= 0 && hedefX < labirent.GetLength(1) && hedefY >= 0 && hedefY < labirent.GetLength(0) && labirent[hedefY, hedefX] != '|')
        {
            oyuncuX = hedefX;
            oyuncuY = hedefY;
            return true;
        }

        return false;
    }

    static void LabirentiHazirla(int level)
    {
        int boyut = 5 + level * 2;
        labirent = new char[boyut, boyut];

        for (int i = 0; i < boyut; i++)
        {
            for (int j = 0; j < boyut; j++)
            {
                labirent[i, j] = ' ';
            }
        }

        for (int i = 0; i < boyut; i++)
        {
            labirent[i, 0] = '|';
            labirent[i, boyut - 1] = '|';
            labirent[0, i] = '|';
            labirent[boyut - 1, i] = '|';
        }

        for (int i = 1; i < boyut - 1; i++)
        {
            for (int j = 1; j < boyut - 1; j++)
            {
                if (random.Next(10) < 2)
                {
                    labirent[i, j] = '|';
                }
            }
        }

        RastgeleBaslat();
        RastgeleCikisBelirle(level); // Çıkış noktasını rastgele belirle

        Console.ForegroundColor = ConsoleColor.White;
    }

    static void LabirentiGoster()
    {
        if (labirentGizli)
        {
            Console.Clear();
            Console.WriteLine("Labirent gizli. Göstermek için 'h' tuşuna basın.");
            return;
        }

        Console.ForegroundColor = ConsoleColor.White;
        for (int i = 0; i < labirent.GetLength(0); i++)
        {
            for (int j = 0; j < labirent.GetLength(1); j++)
            {
                if (i == oyuncuY && j == oyuncuX)
                    Console.Write('©');
                else
                    Console.Write(labirent[i, j]);
            }
            Console.WriteLine();
        }

        Console.ResetColor();

        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write("Kolay/");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Orta/");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("Zor");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Sıkışılırsa Veya Çıkmak İmkansızsa 'R' Tuşuna Basıp Hareket Ediniz ");
        Console.ResetColor();
    }

    static void RastgeleCikisBelirle(int level)
    {
        int boyut = 5 + level * 2;

        do
        {
            cikisX = random.Next(1, boyut - 1);
            cikisY = random.Next(1, boyut - 1);
        } while (labirent[cikisY, cikisX] != ' ');

        labirent[cikisY, cikisX] = ' ';
    }
}
