﻿
//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================


namespace YmtSystem.Domain.Shard.Validator
{
    /// <summary>
    /// Entity Validator Factory
    /// </summary>
    public static class EntityValidatorFactory
    {
        #region Members

        static IEntityValidatorFactory _factory = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Set the  log factory to use
        /// </summary>
        /// <param name="factory">Log factory to use</param>
        public static void SetCurrent(IEntityValidatorFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Createt a new <paramref name="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILog"/>
        /// </summary>
        /// <returns>Created ILog</returns>
        public static IEntityValidator CreateValidator()
        {
            if (_factory == null)
            {
                EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
            }
            return (_factory != null) ? _factory.Create() : null;
        }
        #endregion
    }
}