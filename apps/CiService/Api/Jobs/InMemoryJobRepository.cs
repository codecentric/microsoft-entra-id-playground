using System.Collections.Immutable;

namespace CiService.Api.Jobs;

public class InMemoryJobRepository
{
    private readonly Dictionary<string, ImmutableHashSet<Job>> _jobsByUserId = new();
    private readonly Dictionary<Guid, Job> _jobsByJobId = new();

    public Job CreateJob(string userId, CreateJobCommand command)
    {
        var job = new Job()
        {
            Id = Guid.NewGuid(),
            CreatedTime = DateTimeOffset.UtcNow,
            RepositoryName = command.RepositoryName,
            ShellCommand = command.ShellCommand
        };

        _jobsByUserId.TryGetValue(userId, out var userJobs);
        _jobsByUserId[userId] = (userJobs ?? ImmutableHashSet<Job>.Empty).Add(job);

        _jobsByJobId[job.Id] = job;

        return job;
    }

    public IReadOnlySet<Job> GetAllByUserId(string userId)
    {
        _jobsByUserId.TryGetValue(userId, out var userJobs);
        return userJobs ?? ImmutableHashSet<Job>.Empty;
    }

    public Job? Get(Guid jobId, string userId)
    {
        if (!_jobsByUserId.TryGetValue(userId, out var userJobs))
        {
            return null;
        }

        _jobsByJobId.TryGetValue(jobId, out var job);

        if (job == null || !userJobs.Contains(job))
        {
            return null;
        }

        return job;
    }
}