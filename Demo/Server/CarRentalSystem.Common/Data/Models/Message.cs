namespace CarRentalSystem.Common.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    public class Message
    {
        private string serializedData;

        public Message(object data)
            => this.Data = data;

        private Message()
        {
        }

        public int Id { get; private set; }

        public Type Type { get; private set; }

        public bool Published { get; private set; }

        public void MarkAsPublished() => this.Published = true;

        [NotMapped]
        public object Data
        {
            // Make sure you ignore null values!
            get => JsonConvert.DeserializeObject(
                this.serializedData, this.Type, 
                new JsonSerializerSettings{NullValueHandling = NullValueHandling.Ignore});

            set
            {
                this.Type = value.GetType();
                
                this.serializedData = JsonConvert.SerializeObject(
                    value,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
        }
    }
}
