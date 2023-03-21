using System.Threading.Tasks;

namespace PolicyService.Messaging;

public interface IEventPublisher
{
    Task PublishMessage<T>(T msg);
}