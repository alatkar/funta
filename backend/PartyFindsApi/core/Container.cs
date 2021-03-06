﻿// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using Azure.Storage.Blobs;
using System;

namespace PartyFindsApi.core
{
    /// <summary>
    /// Container of objects to be used in the application.
    /// Dependency injection is not sufficient as we need different objects of same type for 
    /// handling cosmod db access.
    /// </summary>
    public class Container : IDisposable
    {
        //Repositories
        public IRepository listingsRepo { get; set; }

        public IRepository notificationsRepo { get; set; }

        public IRepository messageRepo { get; set; }

        public IRepository userRepo { get; set; }

        public BlobContainerClient uploadsContainer { get; set; }

        private static Container instance = null;

        private static readonly object @lock = new object();

        private Container()
        {
        }

        public static Container Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (@lock)
                    {
                        if (instance == null)
                        {
                            instance = new Container();
                        }
                    }
                }
                return instance;
            }
        }

        public void Dispose()
        {
            listingsRepo?.Dispose();
            notificationsRepo?.Dispose();
            messageRepo?.Dispose();
            userRepo?.Dispose();
        }
    }
}
