using System;
using System.Collections.Generic;
using System.Text;

namespace WebFramework.Application.Models
{
    public class GetByIdRequest
    {
        public int Id { get; set; }

        public Guid GiudId { get; set; }

        public GetByIdRequest(int id)
        {
            Id = id;
        }

        public GetByIdRequest(Guid giudId)
        {
            GiudId = giudId;
        }
    }
}
