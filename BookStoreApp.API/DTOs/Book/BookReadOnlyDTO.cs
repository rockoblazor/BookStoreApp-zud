﻿namespace BookStoreApp.API.DTOs.Book
{
    public class BookReadOnlyDTO : BaseDto
    {
        public string Title { get; set; }

        public string Image { get; set; }
        public decimal Price { get; set; }
        public int AuthorId { get; set; }

        public string AuthorName { get; set; }


    }
}
