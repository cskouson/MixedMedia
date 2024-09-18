using System;
using System.Collections.Generic;

namespace MixedMedia.Data.Entities
{
    
    public class ImageEntity
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}