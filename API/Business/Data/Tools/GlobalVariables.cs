﻿using Business.Data.Tools.Interfaces;



namespace Business.Data
{
    // singleton

    public class GlobalVariables : IGlobalVariables
    {

        private bool _dbState = true;





        public bool DBState
        {
            get { return _dbState; }
            set { _dbState = value; }
        }

    }
}