﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Microsoft.AspNet;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SpotMapProject.Models;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
namespace SpotMapProject.Controllers
{

   // [Authorize(Roles = "Admin,Moderator")]
    public class DataBaseController : Controller
    {
        // private DataBaseEntities spot_entity = new DataBaseEntities(); //spot entity
        private SpotEntities spot_entity = new SpotEntities();
        private UsersDBEntity user_entity = new UsersDBEntity();//users entity
        private SpotCommentEntity spotcom_entity = new SpotCommentEntity();
        private SpotRatingEntity spotrat_entity = new SpotRatingEntity();
        private EditSpotEntity spotreq_entity = new EditSpotEntity();
        private AspNetPhotoEntity spotphoto_entity = new AspNetPhotoEntity();
        private FavoriteSpotEntity spotfav_entity = new FavoriteSpotEntity();
       public void AddSpot(AspNetSpot model)//Add Spot As Admin or moder
        {

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
           
            model.Id = GetLastSpotID() + 1;
             
               model.added = true;
               model.author = user.UserName;
               spot_entity.AspNetSpots.Add(model);
               spot_entity.SaveChanges();
            
        }

        public void AddSpotRequest(AspNetSpot model)//Make Spot request
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            model.author = user.UserName;
            model.Id = GetLastSpotID() + 1;
            model.visible = false;
            model.added = false;
            spot_entity.AspNetSpots.Add(model);
            spot_entity.SaveChanges();

        }

        public int GetLastSpotID()
        {
            int max = Convert.ToInt32(spot_entity.AspNetSpots.Max(p => p.Id));
            return max;
        }
       [Authorize]
        public List<AspNetSpot> GetAllVisibleSpots()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            List < AspNetSpot > spots = new List<AspNetSpot>();
            string username = user.UserName;
           
            spots = spot_entity.AspNetSpots.Where(x => x.visible == true && x.@public == true && x.added == true)
                         .ToList();

            spots.AddRange(spot_entity.AspNetSpots.Where(x => x.author == username && x.@public == false)
                         .ToList());
            return spots;
        }

        



        public List<AspNetSpot> GetAllSpots()
        {
            List<AspNetSpot> spots = new List<AspNetSpot>();

            spots = spot_entity.AspNetSpots.ToList();

            return spots;
        }
        public List<AspNetUser> GetAllUsers()
        {
            List < AspNetUser > users = new List<AspNetUser>();
            users = user_entity.AspNetUsers.ToList();
            
            return users;
        }

        public void DeleteUserByID(string id)
        {
            AspNetUser user = user_entity.AspNetUsers.Find(id);

            if (user == null)
            {


            }
            else
            {
                user_entity.AspNetUsers.Remove(user);
                user_entity.SaveChanges();
            }


        }

        public AspNetUser GetUserDataByID(string id)
        {
            AspNetUser user = new AspNetUser();
            user = user_entity.AspNetUsers.Find(id);
            if (user == null)
            {


            }
            return user;
        }
        public void UpdateUserInfo(AspNetUser model)
        {
            AspNetUser user = new AspNetUser();
            try
            {
                user = user_entity.AspNetUsers.Find(model.Id);
                if (user != null)
                {
                    user= model;
                    user_entity.SaveChanges();





                }

            }
            catch (Exception ex)
            {

            }
        }

        public  AspNetSpot  GetSpotDataByID(string id)
        {
            AspNetSpot result = new AspNetSpot();
            result =  spot_entity.AspNetSpots.Find(Convert.ToInt32(id));
            return result;
        }


        public int GetLastCommentID()
        {
            int max = Convert.ToInt32(spotcom_entity.AspNetSpotComments.Max(p => p.Id));
            return max;
        }

       public void AddComment(string id, string text)
        {

            string username = "";

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            username = user.UserName;



            AspNetSpotComment comment = new AspNetSpotComment();

            int buff = GetLastCommentID();
            buff += 1;

            comment.Id = buff;
            comment.spot_id = id;
            comment.text = text;
            comment.username = username;
            comment.date = DateTime.Now;
            spotcom_entity.AspNetSpotComments.Add(comment);
            spotcom_entity.SaveChanges();
        }

        public List<string> GetSpotCommentsByID(string id)
        {
            List<string> comments = new  List<string>();
            
            comments = spotcom_entity.AspNetSpotComments.Where(x => x.spot_id == id)
                         .Select(x => x.text).ToList();

            

            return comments;

        }

        public List<string> GetCommentUsernameByID(string id)
        {
            List<string> usernames = new List<string>();

            usernames = spotcom_entity.AspNetSpotComments.Where(x => x.spot_id == id)
                         .Select(x => x.username).ToList();

            return usernames;
        }

        public void RateSpot(string id,string value)
        {
            if (value != null)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());


                AspNetSpotRating rating = new AspNetSpotRating();
                if (GetUserRatingByID(Convert.ToInt32(id)))
                {
                    int ID = Convert.ToInt32(id);

                    AspNetSpotRating spotrat = spotrat_entity.AspNetSpotRatings.FirstOrDefault(x => x.spot_id == ID && x.username == user.UserName);
                    spotrat.value = Convert.ToInt32(value);

                    spotrat_entity.SaveChanges();



                }
                else
                {
                    rating.Id = GetLastRatingID() + 1;
                    rating.spot_id = Convert.ToInt32(id);
                    rating.username = user.UserName;
                    rating.value = Convert.ToInt32(value);
                    spotrat_entity.AspNetSpotRatings.Add(rating);
                    spotrat_entity.SaveChanges();
                }
            }
            else
            {

            }
        }

        public int GetLastRatingID()
        {
            int max = Convert.ToInt32(spotrat_entity.AspNetSpotRatings.Max(p => p.Id));
            return max;
        }

        public int GetSpotRatingValueByID(string id)
        {
            int ID = Convert.ToInt32(id);
            List<int> values = new List<int>();
            int result = 0;
            int buff = 0;
           values = spotrat_entity.AspNetSpotRatings.Where(x => x.spot_id == ID)
            .Select(x => x.value).ToList();
            foreach(int val in values)
            {
                buff += val;
            }
            if (values.Count != 0)
            {
                result = buff / values.Count;
            }
            return result;
        }
        public bool GetUserRatingByID(int id) //Check if User Rated Current Spot
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            bool rated = false; 
            List<string> username = spotrat_entity.AspNetSpotRatings.Where(x => x.spot_id == id &&  x.username == user.UserName)
            .Select(x => x.username).ToList();
            if(username.Count<=0)
            {
               
            }
            else
            {
                    rated = true;
                
            }


            return rated;
        }

        public List<AspNetSpot> GetSpotRequests()
        {
            List<AspNetSpot> spots = new List<AspNetSpot>();
            spots = spot_entity.AspNetSpots.Where(x => x.added == false).ToList();

            return spots; 
        }

        public void AcceptSpotRequest(string id)
        {
            
            int ID = Convert.ToInt32(id);
            AspNetSpot spot = spot_entity.AspNetSpots.FirstOrDefault(x => x.Id == ID);
            spot.added = true;
            spot.visible = true;
            spot_entity.SaveChanges();

        }

        public void UpdateSpot(AspNetSpot model) {

            AspNetSpot spot = spot_entity.AspNetSpots.FirstOrDefault(x => x.Id == model.Id);
            spot.title = model.title;
            spot.desc = model.desc;
            spot.lat = model.lat;
            spot.lon = model.lon;
            if (model.visible != null)
            {
                spot.visible = model.visible;
            }
            if (model.@public != null)
            {
                spot.@public = model.@public;
            }
            spot.added = true;
            if (model.author != null)
            {
                spot.author = model.author;
            }
            spot_entity.SaveChanges();


        }

        public List<DateTime?> GetSpotCommentDateByID(string id)
        {
            List<DateTime?> dates = new List<DateTime?>();

          dates = spotcom_entity.AspNetSpotComments.Where(x => x.spot_id == id)
                         .Select(x => x.date).ToList();

            return dates;
        }

        public void DeleteSpot(string id)
        {
            int iD = Convert.ToInt32(id);
            AspNetSpot spot = spot_entity.AspNetSpots.Find(iD);
            if (spot != null)
            {
                DeleteSpotEditors(id);
                DeleteSpotComments(id);
                DeleteSpotRatings(id);
                spot_entity.AspNetSpots.Remove(spot);
                spot_entity.SaveChanges();
            }
        }



        public void DeleteSpotEditors(string id)
        {
            List<AspNetSpotEdit> spots_ed = new List<AspNetSpotEdit>();
            int ID = Convert.ToInt32(id);
            spots_ed = spotreq_entity.AspNetSpotEdits.Where(x => x.spot_id == id).ToList();
            foreach (AspNetSpotEdit sp in spots_ed)
            {
               spotreq_entity.AspNetSpotEdits.Remove(sp);
            }
            spotreq_entity.SaveChanges();
        }

        public void DeleteSpotComments(string id)
        {
            
            List<AspNetSpotComment> spots_comm = new List<AspNetSpotComment>();
            int ID = Convert.ToInt32(id);
            spots_comm = spotcom_entity.AspNetSpotComments.Where(x => x.spot_id == id).ToList();
            foreach(AspNetSpotComment sp in spots_comm)
            {
                spotcom_entity.AspNetSpotComments.Remove(sp);
            }
            spotcom_entity.SaveChanges();
        }

        public void DeleteSpotRatings(string id)
        {
            int iD = Convert.ToInt32(id);
            List<AspNetSpotRating> spots_rat = new List<AspNetSpotRating>();
            spots_rat = spotrat_entity.AspNetSpotRatings.Where(x => x.spot_id == iD).ToList();
            foreach (AspNetSpotRating sp in spots_rat)
            {
                spotrat_entity.AspNetSpotRatings.Remove(sp);
            }
            spotrat_entity.SaveChanges();
        }

        public List<AspNetSpotEdit> ShowEditRequests()
        {
            return spotreq_entity.AspNetSpotEdits.Where(x => x.active == false).ToList();
        }

        public bool CheckUserEditor(string id)
        {
            int iD = Convert.ToInt32(id);
            List<AspNetSpotEdit> ed = new List<AspNetSpotEdit>();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ed = spotreq_entity.AspNetSpotEdits.Where(x => x.username == user.UserName && x.active == true && x.spot_id == id).ToList();
            if (ed.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public int GetLastEditRequestID()
        {
            int max = Convert.ToInt32(spotreq_entity.AspNetSpotEdits.Max(p => p.Id));
            return max;
        }

        public int AddEditRequest(string id)
        {
            AspNetSpotEdit edit = new AspNetSpotEdit();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            List<AspNetSpotEdit> ed = new List<AspNetSpotEdit>();
            ed = spotreq_entity.AspNetSpotEdits.Where(x => x.username == user.UserName && x.spot_id == id).ToList();
            if (ed.Count > 0){

                
            }
            else
            {


                edit.Id = GetLastEditRequestID() + 1;
                edit.spot_id = id;
                edit.username = user.UserName;

                edit.active = false;

                spotreq_entity.AspNetSpotEdits.Add(edit);
                spotreq_entity.SaveChanges();
               
            }
            return ed.Count;
        }

       public void AcceptUserEditRequest(string id)
        {

            int iD = Convert.ToInt32(id);
          AspNetSpotEdit edit = spotreq_entity.AspNetSpotEdits.FirstOrDefault(x => x.Id == iD);
            if (edit != null ){
                edit.active = true;
                spotreq_entity.SaveChanges();
            }
        }

        public void AllowEdit(string id)
        {
            AspNetSpotEdit edit = new AspNetSpotEdit();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            edit.Id = GetLastEditRequestID() + 1;
            edit.spot_id = id;
            edit.username = user.UserName;

            edit.active = true;

            spotreq_entity.AspNetSpotEdits.Add(edit);
            spotreq_entity.SaveChanges();
        }


        public void AddPhotoPath(string spot_id, string photo_path)
        {
            AspNetSpotPhoto model = new AspNetSpotPhoto();
            int buff = GetLastPhotoPathID();
            buff += 1;


            model.Id = buff;
            model.spot_id = spot_id;
            model.path = photo_path;
            spotphoto_entity.AspNetSpotPhotos.Add(model);
            spotphoto_entity.SaveChanges();
        }



        public int GetLastPhotoPathID()
        {



            int max = Convert.ToInt32(spotphoto_entity.AspNetSpotPhotos.Max(p => p.Id));
            return max;
        }

        public List<string> GetAllSpotPhotosByID(string spot_id)
        {
            List<string> photos = new List<string>();

            photos.AddRange(spotphoto_entity.AspNetSpotPhotos.Where(x => x.spot_id == spot_id).Select(x => x.path).ToList());


            return photos;
        }

        public void AddToFavoriteSpot(string id)
        {
            AspNetSpotFavorite model = new AspNetSpotFavorite();
            List<AspNetSpotFavorite> buff = new List<AspNetSpotFavorite>();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            buff = spotfav_entity.AspNetSpotFavorites.Where(x => x.spot_id == id && x.username == user.UserName).ToList();
            if (buff.Count > 0)//delete from fav
            {
                spotfav_entity.AspNetSpotFavorites.Remove(buff[0]);
                spotfav_entity.SaveChanges();
            }
            else//add to fav
            {



                model.Id = GetLastFavoriteID() + 1;
                model.spot_id = id;
                model.username = user.UserName;
                spotfav_entity.AspNetSpotFavorites.Add(model);
                spotfav_entity.SaveChanges();
            }
        }

        public int GetLastFavoriteID()
        {
           
            int max = Convert.ToInt32(spotfav_entity.AspNetSpotFavorites.Max(p => p.Id));
            return max;
        }

        public List<AspNetSpot> GetFavoriteSpotList()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            List<AspNetSpotFavorite> fav = new List<AspNetSpotFavorite>();
            List<AspNetSpot> fav_spots = new List<AspNetSpot>();
            fav = spotfav_entity.AspNetSpotFavorites.Where(x => x.username == user.UserName).ToList();
            foreach(AspNetSpotFavorite f in fav)
            {
                int buff = Convert.ToInt32(f.spot_id);
                fav_spots.AddRange(spot_entity.AspNetSpots.Where(x => x.Id == buff).ToList());
            }

            return fav_spots;
        }

        public bool CheckInFavoriteByID(string id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            List<AspNetSpotFavorite> buff = new List<AspNetSpotFavorite>();
            buff = spotfav_entity.AspNetSpotFavorites.Where(x => x.spot_id == id && x.username == user.UserName).ToList();
            if (buff.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
   
}