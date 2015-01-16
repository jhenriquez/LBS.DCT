﻿using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LBS.DCT.JsonRPC.Requests
{
    public class SetOutputRequest : Request
    {
        [Required]
        public string SessionKey { get; set; }
        
        public string IdType { get; set; }

        [Required]
        public string ID { get; set; }

        [Required]
        public string OType { get; set; }

        [Required]
        public string OutputIndex { get; set; }

        public bool State { get; set; }

        public SetOutputRequest(String url)
            : base(url, Guid.NewGuid().ToString())
        {
            Method = "setOutput";
            IdType = "imei";
        }

        public SetOutputRequest(String url, String sessionKey)
            : this(url)
        {
            SessionKey = sessionKey;
        }

        public override dynamic Execute()
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this), results);

            if (results.FirstOrDefault() != null)
            {
                throw new ArgumentNullException(String.Join(",", results.Select(v => v.MemberNames.FirstOrDefault())));
            }

            Parameters = new JArray(new object[] { JValue.CreateString(SessionKey), JValue.CreateString(IdType), JValue.CreateString(ID),
                                                JValue.CreateString(OType), JValue.CreateString(OutputIndex), JValue.CreateString(State.ToString()) });

            return base.Execute();
        }
    }
}