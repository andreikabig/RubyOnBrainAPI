using Microsoft.EntityFrameworkCore;
using RubyOnBrain.API.Models;
using RubyOnBrain.Data;
using RubyOnBrain.Domain;
using System.Text.Json;

namespace RubyOnBrain.API.Services
{
    public class RecommendationService
    {
        //private DataContext db;
        private readonly string uploadPath;
        private List<UserCoursePredictsDTO>? lastPredicts;

        public RecommendationService()
        {
            //ReConnectToDb();

            lastPredicts = new List<UserCoursePredictsDTO>();

            uploadPath = $"wwwroot/uploads/data/";

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);
        }

        // Нужен ли этот метод?
        //public void ReConnectToDb()
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        //    var options = optionsBuilder.UseSqlServer(@"Server= (localdb)\\MSSQLLocalDB; Database=RubyOnBrainDB;Trusted_Connection=True;")
        //        .Options;

        //    db = new DataContext(options);
        //}

        //public List<UserCourse>? GetCurrentRatings()
        //{
        //    ReConnectToDb();
        //    return db.UserCourses.ToList();
        //}

        public bool SavePredicts(List<UserCoursePredictsDTO> predicts) 
        {
            //ReConnectToDb();
            try
            {
                lastPredicts = predicts;
                SaveToFile(predicts);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<string>? GetFileNames()
        {
            var fileNames = Directory.GetFiles(uploadPath, "*.json").ToList();
            return fileNames;
        }

        public async void SaveToFile(List<UserCoursePredictsDTO> predicts)
        {
            //ReConnectToDb();
            string fullPath = uploadPath + $"recsyspredicts_{DateTime.Now.Date.ToShortDateString()}.json";
            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            { 
                await JsonSerializer.SerializeAsync(fs, predicts);
            }
        }

        public bool LoadRatingsFromFile(string fileName)
        {
            //ReConnectToDb();
            if (File.Exists(uploadPath + $"/{fileName}"))
            {
                try
                {
                    ReadFromFile(uploadPath + $"/{fileName}");
                    return true;
                }
                catch { }
            }
            
            return false;
        }

        public async void ReadFromFile(string filePath)
        {
            //ReConnectToDb();
            List<UserCoursePredictsDTO>? predicts;
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                predicts = await JsonSerializer.DeserializeAsync<List<UserCoursePredictsDTO>>(fs);
            }

            if (predicts != null)
                lastPredicts = predicts;
        
        }

        public UserCoursePredictsDTO? GetPredictForUser(int id)
        {
            //ReConnectToDb();
            UserCoursePredictsDTO? ucpDTO = null;
            if (lastPredicts != null)
            {
                var ratings = lastPredicts.Where(u => u.UserId == id).Select(x => x.Rating);
                if (ratings.Count() > 0)
                {
                    var maxRating = ratings.Max();

                    ucpDTO = lastPredicts.FirstOrDefault(uc => uc.UserId == id && uc.Rating == maxRating);
                }
                
            }
            return ucpDTO;
        }
    }
}
