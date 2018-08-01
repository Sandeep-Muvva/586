using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapperCSCI586
{
    class SaveData
    {

        public static void SafeDataSQLDatabaseGameSpot(GameSpotGame _game)
        {
            try
            {
                var db = new UpdateModelII();
                db.GameSpotGames.Add(_game);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                //foreach (var validationErrors in e.EntityValidationErrors)
                //{
                //    foreach (var validationError in validationErrors.ValidationErrors)
                //    {
                //        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                //    }
                //}
                string h = e.ToString();
            }
                

        }

        public static void SafeDataSQLDatabaseMetaCritics(MetaCriticsGame _game)
        {
            try
            {
                var db = new UpdatedModelGames();
                db.MetaCriticsGames.Add(_game);
                db.SaveChanges();
            }
            catch (DbUpdateException e)
            {

                //foreach (var validationErrors in e.GetBaseException)
                //{
                //    foreach (var validationError in validationErrors.ValidationErrors)
                //    {
                //        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                //    }
                //}

               string h = e.GetBaseException().ToString();
                h = h;
            }


        }

        public static void SafeDataSQLDatabaseIGN(IGNGame _game)
        {
            try
            {
                var db = new UpdateModelII();
                db.IGNGames.Add(_game);
                db.SaveChanges();
            }
            catch (DbUpdateException e)
            {

                //foreach (var validationErrors in e.GetBaseException)
                //{
                //    foreach (var validationError in validationErrors.ValidationErrors)
                //    {
                //        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                //    }
                //}

                string h = e.GetBaseException().ToString();
                h = h;
            }


        }
    }
}
