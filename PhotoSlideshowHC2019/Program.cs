using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoSlideshowHC2019
{
    internal class Program
    {
        public static int logFI;
        public static int logPunteggio = 0;

        static void Main(string[] args)
        {
            string appPath = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            string file = File.ReadAllText(Path.Combine(appPath, @"..\Files", "c_memorable_moments.txt"));
            string[] fileSplittato = file.Split('\n');

            List<Photo> photos = DeserializeFile(fileSplittato);
            Dictionary<int, List<int>> slideShow = GeneraSlideshow(photos);

            Console.WriteLine("Punteggio finale:" + logPunteggio);
            Console.ReadKey();
        }

        private static Dictionary<int, List<int>> GeneraSlideshow(List<Photo> photos)// Invece di photos verrà passato slides (contenente foto H e combinazioni di foto V)
        {
            List<Photo> photosCopy = photos.GetRange(0, photos.Count);

            Dictionary<int, List<int>> slideShow = new Dictionary<int, List<int>>();
            slideShow.Add(0, new List<int>() { photosCopy[0].Id });
            photosCopy.Remove(photosCopy[0]);

            int idPhotoMaxFI = 0;
            bool trovataProssimaSlide;
            for (int i = 0; i < photos.Count; i++)
            {
                trovataProssimaSlide = false;
                int maxFattoreInteresse = 0;
                int fattoreInteresse;
                int tagComune;
                int tagP1 = photos[idPhotoMaxFI].tags.Count;
                int tagP2;
                for (int j = 0; j < photosCopy.Count; j++)
                {
                    tagP2 = photosCopy[j].tags.Count;
                    tagComune = CalcolaTag(photos[idPhotoMaxFI].tags, photosCopy[j].tags, ref tagP1, ref tagP2);

                    List<int> tmp = new List<int> { tagComune, tagP1, tagP2 };
                    fattoreInteresse = tmp.Min();
                    if (fattoreInteresse > maxFattoreInteresse)
                    {
                        maxFattoreInteresse = fattoreInteresse;
                        idPhotoMaxFI = photosCopy[j].Id;

                        trovataProssimaSlide = true;
                        logFI = maxFattoreInteresse; //Solo per logging
                    }
                }

                if (trovataProssimaSlide == true)
                {
                    slideShow.Add(i + 1, new List<int>() { idPhotoMaxFI });
                    photosCopy.Remove(photosCopy.Where(p => p.Id == idPhotoMaxFI).FirstOrDefault());

                    logPunteggio += logFI;
                    Console.WriteLine(logFI);
                }
            }

            return slideShow;
        }

        private static int CalcolaTag(List<string> tags1, List<string> tags2, ref int tagP1, ref int tagP2)
        {
            int tagComune = 0;
            for (int i = 0; i < tags1.Count; i++)
            {
                if (tags2.Contains(tags1[i]))
                {
                    tagComune++;
                }
            }

            tagP1 = tagP1 - tagComune;
            tagP2 = tagP2 - tagComune;
            return tagComune;
        }

        private static List<Photo> DeserializeFile(string[] fileSplittato)
        {
            List<Photo> photos = new List<Photo>();
            int numeroFoto = Convert.ToInt32(fileSplittato[0]);
            for (int i = 1; i <= numeroFoto; i++)
            {
                string[] rigaSplittata = fileSplittato[i].Split(' ');
                Photo photo = new Photo()
                {
                    Id = i - 1,
                    Orientamento = Convert.ToChar(rigaSplittata[0]),
                    NumberOfTags = Convert.ToInt32(rigaSplittata[1]),
                    tags = new List<string>()
                };

                for (int j = 2; j < photo.NumberOfTags + 2; j++)
                {
                    photo.tags.Add(rigaSplittata[j]);
                }

                photos.Add(photo);
            }

            return photos;
        }
    }
}
