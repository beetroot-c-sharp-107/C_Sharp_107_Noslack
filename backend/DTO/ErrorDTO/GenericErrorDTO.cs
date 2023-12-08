using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace backend.DTO.UserControllerDTO
{
    public class GenericErrorDTO
    {
        public string RequestID { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorType { get; set; }
    }
}
