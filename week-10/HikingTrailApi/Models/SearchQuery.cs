﻿using System;

namespace HikingTrailApi.Models
{
  public class SearchQuery
  {
    public int Id { get; set; }
    public string SearchTerm { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
  }
}