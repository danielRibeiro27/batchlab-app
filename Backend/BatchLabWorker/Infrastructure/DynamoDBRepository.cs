using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Threading.Tasks;

public class DynamoDBRepository
{
    private readonly IAmazonDynamoDB _dynamoDbClient;
    private const string TableName = "Jobs";

    public DynamoDBRepository(IAmazonDynamoDB dynamoDbClient)
    {
        _dynamoDbClient = dynamoDbClient;
    }

    public async Task UpdateJobStatusAsync(string jobId, string status)
    {
        var updateItemRequest = new UpdateItemRequest
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "Id", new AttributeValue { S = jobId } }
            },
            UpdateExpression = "SET #s = :status",
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                { "#s", "Status" }
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":status", new AttributeValue { S = status } }
            }
        };

        await _dynamoDbClient.UpdateItemAsync(updateItemRequest);
    }
}