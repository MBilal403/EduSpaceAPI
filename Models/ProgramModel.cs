﻿namespace EduSpaceAPI.Models
{
    public class ProgramModel
    {
        public int ProgramId { get; set; }
        public string? ProgramName { get; set; }
        public string? ProgramShortName { get; set; }
        public int DepartFId { get; set; }
        public bool Status { get; set; }
    }
}
