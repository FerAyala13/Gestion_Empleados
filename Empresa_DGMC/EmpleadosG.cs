using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Empresa_DGMC
{
    public class EmpleadosG
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Nombre")]
        [BsonRequired]
        public string Nombre { get; set; }

        [BsonElement("Edad")]
        [BsonRequired]
        public int Edad { get; set; }

        [BsonElement("Ciudad")]
        [BsonRequired]
        public string Ciudad { get; set; }

        [BsonElement("Correo")]
        public string Correo { get; set; }
        
        [BsonElement("FechaPrueba")]
        [BsonIgnoreIfNull]
        public DateTime? FechaPrueba { get; set; }
        
        // Constructor sin argumentos necesario para la serialización de MongoDB
        public EmpleadosG()
        {
            // Asignar un ID por defecto para nuevas instancias
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}