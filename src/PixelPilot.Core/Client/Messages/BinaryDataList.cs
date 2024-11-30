namespace PixelPilot.Client.Messages;

public class BinaryDataList
{
    private dynamic[] _data { get; set; }
    public IReadOnlyCollection<dynamic> Items => _data;
    
    public BinaryDataList(dynamic[] data)
    {
        _data = data;
    }
    
    public static BinaryDataList FromByteArray(byte[] array)
    {
        return new BinaryDataList(BinaryFieldConverter.ConstructBinaryDataList(array));
    }

    
    public dynamic Get(int index) => _data[index];
    public byte[] ToByteArray()
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        for (int i = 0; i < _data.Length; i++)
        {
            BinaryFieldConverter.WriteTypeBe(writer, _data[i]);
        }

        return memoryStream.ToArray();
    }
}