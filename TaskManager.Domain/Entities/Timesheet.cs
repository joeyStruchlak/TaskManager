namespace TaskManager.Domain.Entities
{
    public class Timesheet
    {
        public int Id { get; private set; }
        public string SubmittedBy { get; private set; }
        public string SupervisorEmail { get; private set; }
        public decimal TotalHours { get; private set; }
        public string Status { get; private set; }
        public DateTime SubmittedAt { get; private set; }
        public DateTime? ReviewedAt { get; private set; }
        public string? ReviewedBy { get; private set; }
        public string? RejectionReason { get; private set; }

        public Timesheet(string submittedBy, string supervisorEmail, decimal totalHours)
        {
            SubmittedBy = submittedBy;
            SupervisorEmail = supervisorEmail;
            TotalHours = totalHours;
            Status = "Pending";
            SubmittedAt = DateTime.UtcNow;
        }

        // methods
        public void Approve(string reviewedBy)
        {
            if (Status != "Pending")
                throw new InvalidOperationException("Only pending timesheets can be approved.");

            Status = "Approved";
            ReviewedBy = reviewedBy;
            ReviewedAt = DateTime.UtcNow;
        }

        public void Reject(string reviewedBy, string reason)
        {
            if (Status != "Pending")
                throw new InvalidOperationException("Only pending timesheets can be rejected.");

            Status = "Rejected";
            ReviewedBy = reviewedBy;
            RejectionReason = reason;
            ReviewedAt = DateTime.UtcNow;
        }
    }
}