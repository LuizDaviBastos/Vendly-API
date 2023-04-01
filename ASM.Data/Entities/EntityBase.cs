using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace ASM.Data.Entities
{
    public interface IEntityBase
    {
        public Guid Id { get; set; }
    }
}
