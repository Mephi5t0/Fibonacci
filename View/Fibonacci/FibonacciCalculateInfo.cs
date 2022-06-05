using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace View.Fibonacci
{
    [DataContract]
    public sealed class FibonacciCalculateInfo
    {
        [DataMember(IsRequired = true)]
        [Range(0, long.MaxValue)]
        public long CurrentNumber { get; set; }
    }
}