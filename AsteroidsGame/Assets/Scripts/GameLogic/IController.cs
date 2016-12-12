
public interface IStateOwner<TState>
{
    TState GetState();
}

public interface IDataOwner<TData>
{
	TData GetData();
}

public interface IDeInitializable
{
    void DeInit();
}

public interface IController<TState> : IDeInitializable, IStateOwner<TState>
{
    void Init(TState state);	
}

public interface IController<TState, T1> : IDeInitializable, IStateOwner<TState>
{
    void Init(TState state, T1 data1);
}

public interface IPausable
{
	IReadOnlyObservableValue<bool> IsPause { get; }
    void Pause();
    void Resume();
}

public interface IPositionable
{
    Vector3ObservableValue Position { get; }
}
