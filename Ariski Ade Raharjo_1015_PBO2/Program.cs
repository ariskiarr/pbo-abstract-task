class Program
{
    static void Main(string[] args)
    {
        // Membuat kemampuan
        IKemampuan seranganListrik = new SeranganListrik();
        IKemampuan perbaikan = new Perbaikan();
        IKemampuan seranganPlasma = new SeranganPlasma();
        IKemampuan pertahananSuper = new PertahananSuper();

        // Membuat robot biasa dan bos
        RobotBiasa robot1 = new RobotBiasa("Robot1", 100, 50, 50);
        RobotBiasa robot2 = new RobotBiasa("Robot2", 100, 50, 50);
        BosRobot bos = new BosRobot("Bos Robot", 200, 100, 75);

        while (true)
        {
            Console.WriteLine("\n=== Pertarungan Robot ===");
            Console.WriteLine("1. Robot1 menyerang Bos");
            Console.WriteLine("2. Robot2 menyerang Bos");
            Console.WriteLine("3. Gunakan kemampuan khusus pada Bos");
            Console.WriteLine("4. Cetak informasi robot");
            Console.WriteLine("5. Keluar");
            Console.Write("Pilih opsi: ");

            string pilihan = Console.ReadLine();
            switch (pilihan)
            {
                case "1":
                    robot1.Serang(bos);
                    break;

                case "2":
                    robot2.Serang(bos);
                    break;

                case "3":
                    GunakanKemampuan(robot1, robot2, bos, seranganListrik, seranganPlasma, perbaikan, pertahananSuper);
                    break;

                case "4":
                    Console.WriteLine("\nInformasi Robot:");
                    robot1.CetakInformasi();
                    robot2.CetakInformasi();
                    bos.CetakInformasi();
                    break;

                case "5":
                    Console.WriteLine("Keluar dari program.");
                    return;

                default:
                    Console.WriteLine("Pilihan tidak valid. Silakan coba lagi.");
                    break;
            }
        }
    }

    static void GunakanKemampuan(RobotBiasa robot1, RobotBiasa robot2, BosRobot bos, IKemampuan seranganListrik, IKemampuan seranganPlasma, IKemampuan perbaikan, IKemampuan pertahananSuper)
    {
        Console.WriteLine("\nPilih kemampuan yang akan digunakan:");
        Console.WriteLine("1. Serangan Listrik");
        Console.WriteLine("2. Serangan Plasma");
        Console.WriteLine("3. Perbaikan");
        Console.WriteLine("4. Pertahanan Super");
        Console.Write("Masukkan pilihan kemampuan: ");

        string pilihanKemampuan = Console.ReadLine();

        Console.WriteLine("\nPilih robot yang akan menggunakan kemampuan:");
        Console.WriteLine("1. Robot1");
        Console.WriteLine("2. Robot2");
        Console.Write("Masukkan pilihan robot: ");

        string pilihanRobot = Console.ReadLine();
        Robot pengguna;

        if (pilihanRobot == "1")
        {
            pengguna = robot1;
        }
        else
        {
            pengguna = robot2;
        }

        switch (pilihanKemampuan)
        {
            case "1":
                pengguna.GunakanKemampuan(seranganListrik, bos);
                seranganListrik.KurangiCooldown();
                break;

            case "2":
                pengguna.GunakanKemampuan(seranganPlasma, bos);
                seranganPlasma.KurangiCooldown();
                break;

            case "3":
                pengguna.GunakanKemampuan(perbaikan, pengguna);
                perbaikan.KurangiCooldown();
                break;

            case "4":
                pengguna.GunakanKemampuan(pertahananSuper, pengguna);
                pertahananSuper.KurangiCooldown();
                break;

            default:
                Console.WriteLine("Pilihan kemampuan tidak valid.");
                break;
        }
    }
}





public abstract class Robot
{
    public string Nama;
    public int Energi;
    public int Armor;
    public int Serangan;

    protected Robot(string nama, int energi, int armor, int serangan)
    {
        Nama = nama;
        Energi = energi;
        Armor = armor;
        Serangan = serangan;
    }

    public virtual void Serang(Robot target)
    {
        int serangan_yangdiberikan = Serangan;
        // mengurangi armor duluan
        if (target.Armor > 0)
        {
            if (serangan_yangdiberikan >= target.Armor) // kalau misalnya serangan itu lebih besar dari pada armor target
            {
                serangan_yangdiberikan -= target.Armor; // akan menguangi armor target
                target.Armor = 0; // armor target jadi 0 untuk menghindari nilai negatif
                Console.WriteLine($"{Nama} menghancurkan armor {target.Nama}.");
            }
            else
            {
                target.Armor -= serangan_yangdiberikan;
                serangan_yangdiberikan = 0; // untuk menghindari nilai negatif
                Console.WriteLine($"{Nama} mengurangi armor {target.Nama} sebanyak {Serangan}.");
            }
        }

        // kalau masih ada sisanya akan mengurangi energi
        if (serangan_yangdiberikan > 0)
        {
            target.Energi -= serangan_yangdiberikan;
            if (target.Energi < 0)
            {
                target.Energi = 0; // untuk memastikan energi tidak negatif /untuk menghindari nilai min
            }
            Console.WriteLine($"{Nama} menyerang {target.Nama}, mengurangi energi sebanyak {serangan_yangdiberikan}.");
        }

        // kalau energinya target 0 mati
        if (target.Energi <= 0)
        {
            Console.WriteLine($"Robot {target.Nama} telah mati.");
        }


    }

    public abstract void GunakanKemampuan(IKemampuan kemampuan, Robot target);

    public void CetakInformasi()
    {
        Console.WriteLine($"Nama Robot\t: {Nama}\nEnergi\t\t: {Energi}\nArmor\t\t: {Armor}\nSerangan\t: {Serangan}");
    }

}

public interface IKemampuan
{
    void Gunakan(Robot pengguna, Robot target);
    void KurangiCooldown();

    bool KeteranganCooldown();
}

public class Perbaikan : IKemampuan
{
    int cooldown;

    public void Gunakan(Robot pengguna, Robot target)
    {
        if (KeteranganCooldown()) // ketika skill cooldown
        {
            Console.WriteLine("Kemampuan perbaikan sedang Cooldown!");
            return;
        }

        Console.WriteLine("Robot sedang melakukan perbaikan.");
        pengguna.Energi += 20;
        cooldown = 2;
    }

    public void KurangiCooldown() // mengurangi cooldown skill giliran
    {
        if (cooldown > 0)
        {
            cooldown -= 1;
        }
    }

    public bool KeteranganCooldown() // memeriksa apakah skill cd
    {
        return cooldown > 0;
    }
}

public class SeranganListrik : IKemampuan
{
    int cooldown;

    public void Gunakan(Robot pengguna, Robot target)
    {
        if (KeteranganCooldown())
        {
            Console.WriteLine("Kemampuan Serangan Listrik sedang cooldown!");
            return;
        }

        int seranganListrik = 20; // damage serangan listrik
        target.Energi -= seranganListrik; // mengurangi energi target
        cooldown = 3;  // set cd // skill yang lain sama
        Console.WriteLine($"Robot {pengguna.Nama} menggunakan Serangan Listrik pada {target.Nama}, mengurangi {seranganListrik} energi.");
    }

    public void KurangiCooldown() // sama
    {
        if (cooldown > 0)
        {
            cooldown -= 1;
        }
    }
    public bool KeteranganCooldown() // sama
    {
        return cooldown > 0; // Mengembalikan true jika masih dalam cooldown
    }
}

public class SeranganPlasma : IKemampuan
{
    int cooldown;

    public void Gunakan(Robot pengguna, Robot target)
    {
        int SeranganPlasma = 20;
        if (cooldown > 0)
        {
            Console.WriteLine("Kemampuan Serangan Plasma Sedang Cooldown!");
            return;
        }
        else if (SeranganPlasma >= target.Energi)
        {
            target.Energi = 0; // Energi target menjadi 0 
            Console.WriteLine($"Robot {pengguna.Nama} Menembakkan Plasma Cannon! pada {target.Nama}");
            Console.WriteLine($"Robot {target.Nama} telah terbunuh karena kehabisan energi disebabkan oleh Plasma Cannon.");
        }
        else
        {
            // Jika serangan tidak membunuh target, kurangi energi target
            target.Energi -= SeranganPlasma;
            Console.WriteLine($"Robot {pengguna.Nama} Menembakkan Plasma Cannon! pada {target.Nama}, mengurangi energi sebesar {SeranganPlasma}.");

        }
        cooldown = 2;
    }
    public void KurangiCooldown()
    {
        if (cooldown > 0)
        {
            cooldown -= 1;
        }
    }
    public bool KeteranganCooldown()
    {
        return (cooldown > 0);
    }
}

public class PertahananSuper : IKemampuan
{
    int cooldown;

    public void Gunakan(Robot pengguna, Robot target)
    {
        if (cooldown > 0)
        {
            Console.WriteLine("Kemampuan Pertahanan Super Sedang Cooldown!");
            return;
        }
        Console.WriteLine("Robot sedang melakukan Pertahanan Super.");
        pengguna.Armor += 20;
        cooldown = 2;
    }
    public void KurangiCooldown()
    {
        if (cooldown > 0)
        {
            cooldown -= 1;
        }
    }
    public bool KeteranganCooldown()
    {
        return (cooldown > 0);
    }
}


public class RobotBiasa : Robot
{
    public RobotBiasa(string nama, int energi, int armor, int serangan)
        : base(nama, energi, armor, serangan) { }

    public override void GunakanKemampuan(IKemampuan kemampuan, Robot target)
    {
        kemampuan.Gunakan(this, target);
    }
}


public class BosRobot : Robot
{
    public BosRobot(string nama, int energi, int armor, int serangan)
        : base(nama, energi, armor, serangan) { }

    public override void GunakanKemampuan(IKemampuan kemampuan, Robot target)
    {
        kemampuan.Gunakan(this, target);
    }

    public void Diserang(Robot penyerang)
    {
        int seranganYangDiterima = penyerang.Serangan;
        // Mengurangi armor terlebih dahulu jika masih ada
        if (Armor > 0)
        {
            if (seranganYangDiterima >= Armor)
            {
                seranganYangDiterima -= Armor;
                Armor = 0;
                Console.WriteLine($"{penyerang.Nama} menghancurkan armor {Nama}.");
            }
            else
            {
                Armor -= seranganYangDiterima;
                seranganYangDiterima = 0;
                Console.WriteLine($"{penyerang.Nama} mengurangi armor {Nama} sebanyak {penyerang.Serangan}.");
            }
        }

        // Mengurangi energi jika ada sisa serangan
        if (seranganYangDiterima > 0)
        {
            Energi -= seranganYangDiterima;
            if (Energi < 0)
            {
                Energi = 0; // Mencegah nilai negatif
            }
            Console.WriteLine($"{penyerang.Nama} menyerang {Nama}, mengurangi energi sebesar {seranganYangDiterima}.");
        }

        // Mengecek apakah energi habis
        if (Energi <= 0)
        {
            Mati();
        }
    }

    // Metode yang akan dipanggil jika energi bos habissssssss
    public void Mati()
    {
        Console.WriteLine($"Bos {Nama} telah mati.");
    }
}
