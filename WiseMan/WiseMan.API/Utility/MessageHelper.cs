﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WiseMan.API.Models;

namespace WiseMan.API.Utility
{
    /// <summary>
    /// To serve as the DAL for message related actions
    /// </summary>
    public static class MessageHelper
    {
        /// <summary>
        /// Creates a new message
        /// </summary>
        /// <param name="body"></param>
        /// <param name="authorId"></param>
        /// <param name="tags"></param>
        public static void CreateMessage(string messageBody, Guid authorId, List<string> tags)
        {
            string tagString = "";
            foreach (var item in tags)
            {
                tagString += item + ",";
            }
            using(WiseMan.API.Data.WiseManEntities db = new Data.WiseManEntities())
            {
                db.CreateMessage(messageBody, tagString, authorId);
            }
        }
    }
}