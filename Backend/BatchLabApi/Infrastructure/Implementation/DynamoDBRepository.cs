using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using BatchLabApi.Domain;
using BatchLabApi.Infrastructure.Interface;

namespace BatchLabApi.Infrastructure.Implementation{
    public class DynamoDBRepository(AmazonDynamoDBClient dynamoDbClient) : IJobsRepository
    {
        private readonly AmazonDynamoDBClient _dynamoDbClient = dynamoDbClient;  

        public async Task CreateAsync(JobEntity entity)
        {
            var request = new PutItemRequest
            {
                TableName = "Jobs",
                Item = new Dictionary<string, AttributeValue>()
                {
                    {"Id", new AttributeValue{ S = entity.Id.ToString() } },
                    {"Description", new AttributeValue{ S = entity.Description } },
                    {"Status", new AttributeValue{ S = entity.Status } },
                    {"CreatedAt", new AttributeValue{ S = entity.CreatedAt.ToString("o") } } //TO-DO: Fuso handling
                }
            };

            var response = await _dynamoDbClient.PutItemAsync(request);
            if(response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Failed to create job in DynamoDB");
            }
        }

        public async Task<List<JobEntity>> GetAllAsync()
        {
            var request = new ScanRequest
            {
                TableName = "Jobs",
                Limit = 25 //TO-DO: Implement batch get item by user id 
            };

            var response = await _dynamoDbClient.ScanAsync(request);
            var jobs = new List<JobEntity>(); //TO-DO: Use DynamoDBContext for mapping
            foreach (var item in response.Items)
            {
                var job = new JobEntity
                {
                    Id = Guid.Parse(item["Id"].S),
                    Description = item["Description"].S,
                    Status = item["Status"].S,
                    CreatedAt = DateTime.Parse(item["CreatedAt"].S)
                };
                jobs.Add(job);
            }
            return jobs;
        }

        public async Task<JobEntity?> GetByIdAsync(string id)
        {
            var request = new GetItemRequest
            {
                TableName = "Jobs",
                Key = new Dictionary<string, AttributeValue>()
                {
                    {"Id", new AttributeValue{ S = id } }
                }
            };

            var response = await _dynamoDbClient.GetItemAsync(request);
            if (!response.IsItemSet)
            {
                return null;
            }

            var item = response.Item;
            var job = new JobEntity
            {
                Id = Guid.Parse(item["Id"].S),
                Description = item["Description"].S,
                Status = item["Status"].S,
                CreatedAt = DateTime.Parse(item["CreatedAt"].S)
            };

            return job; 
        }
    }
}