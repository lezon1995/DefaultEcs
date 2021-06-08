#### [DefaultEcs](DefaultEcs.md 'DefaultEcs')
### [DefaultEcs](DefaultEcs.md#DefaultEcs 'DefaultEcs').[World](World.md 'DefaultEcs.World')
## World.Subscribe&lt;T&gt;(MessageHandler&lt;T&gt;) Method
Subscribes an [MessageHandler&lt;T&gt;(T)](MessageHandler_T_(T).md 'DefaultEcs.MessageHandler&lt;T&gt;(T)') to be called back when a [T](World_Subscribe_T_(MessageHandler_T_).md#DefaultEcs_World_Subscribe_T_(DefaultEcs_MessageHandler_T_)_T 'DefaultEcs.World.Subscribe&lt;T&gt;(DefaultEcs.MessageHandler&lt;T&gt;).T') object is published.  
```csharp
public System.IDisposable Subscribe<T>(DefaultEcs.MessageHandler<T> action);
```
#### Type parameters
<a name='DefaultEcs_World_Subscribe_T_(DefaultEcs_MessageHandler_T_)_T'></a>
`T`  
The type of the object to be called back with.
  
#### Parameters
<a name='DefaultEcs_World_Subscribe_T_(DefaultEcs_MessageHandler_T_)_action'></a>
`action` [DefaultEcs.MessageHandler&lt;](MessageHandler_T_(T).md 'DefaultEcs.MessageHandler&lt;T&gt;(T)')[T](World_Subscribe_T_(MessageHandler_T_).md#DefaultEcs_World_Subscribe_T_(DefaultEcs_MessageHandler_T_)_T 'DefaultEcs.World.Subscribe&lt;T&gt;(DefaultEcs.MessageHandler&lt;T&gt;).T')[&gt;](MessageHandler_T_(T).md 'DefaultEcs.MessageHandler&lt;T&gt;(T)')  
The delegate to be called back.
  
#### Returns
[System.IDisposable](https://docs.microsoft.com/en-us/dotnet/api/System.IDisposable 'System.IDisposable')  
An [System.IDisposable](https://docs.microsoft.com/en-us/dotnet/api/System.IDisposable 'System.IDisposable') object used to unsubscribe.

Implements [Subscribe<T>(MessageHandler<T>)](IPublisher_Subscribe_T_(MessageHandler_T_).md 'DefaultEcs.IPublisher.Subscribe&lt;T&gt;(DefaultEcs.MessageHandler&lt;T&gt;)')  