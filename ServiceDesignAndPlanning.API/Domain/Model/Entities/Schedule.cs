namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Entities;

public class Schedule
{
    public string ScheduleId { get; private set; }
    public Guid TechnicianId { get; private set; }
    public string Day { get; private set; }
    public string StartTime { get; private set; }
    public string EndTime { get; private set; }

    public Schedule(string scheduleId, Guid  technicianId, string day, string startTime, string endTime)
    {
        ScheduleId = scheduleId;
        TechnicianId = technicianId;
        Day = day;
        StartTime = startTime;
        EndTime = endTime;
    }
    public void UpdateSchedule(string newDay, string newStartTime, string newEndTime)
    {
        Day = newDay;
        StartTime = newStartTime;
        EndTime = newEndTime;
    }
    

}