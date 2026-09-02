using ParameterDataLib;
using TelemetryDeviceV1.Dataflow.Stages.Helpers;
using TelemetryDeviceV1.ICD;
using TelemetryDeviceV1.ICD.Enums;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public class DecoderStage : IDecoderStage
    {
        private readonly IcdDeserializer _deserializer;

        public DecoderStage(IcdDeserializer deserializer)
        {
            _deserializer = deserializer;
        }
        
        public IEnumerable<ParameterData> Decode(IEnumerable<byte> data)
        {
            if (data == Enumerable.Empty<byte>())
            {
                yield break;
            }

            byte[] dataArr = data.ToArray();

            ulong timestampMS = BitConverter.ToUInt64(dataArr);

            byte correlatorByte = dataArr[8];

            dataArr = dataArr[9..];

            foreach (IcdParameter p in _deserializer.CorrelatorGroups[correlatorByte])
            {
                byte[] result = dataArr.Skip(p.Offset).Take(p.Size).ToArray();

                ValueBase value;

                if (p.Type == ParameterDataType.Float64)
                {
                    value = new DoubleValue(BitMaskService.GetFloat64(result, p.BitMask));
                }
                else
                {
                    value = new IntValue(BitMaskService.GetInt32(result, p.BitMask, p.Type));
                }

                yield return new ParameterData()
                {
                    Name = p.Name,
                    Type = p.Type == ParameterDataType.Float64 ? DataType.Double : DataType.Int,
                    Units = p.Unit,
                    Value = value,
                    TimestampMS = timestampMS
                };
            }
        }
    }
}
