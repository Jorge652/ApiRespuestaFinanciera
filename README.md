# ApiRespuestaFinanciera
## Ejecución
1. Actualiza la conexión en `appsettings.json`:
   "ConnectionStrings": {
     "DefaultConnection": "Server=ANDRES;Database=ApiRespuesta;Trusted_Connection=True;MultipleActiveResultSets=true"
   }

2.Compilar y ejecutar mediante la herramienta de Visual Studio

   Probar ENDPOINTS
   1. POST /api/creditos/respuesta
       APROBADO (éxito)
       {
          "numeroSolicitud": "SOL-2025-001245",
          "estado": "APROBADO",
          "montoAprobado": 18500.00,
          "tasa": 14.5,
           "plazo": 48,
          "observaciones": "Cliente califica sin restricciones",
          "fechaRespuesta": "2025-07-10T10:25:00"
        }
       APROBADO (error)
       {
          "numeroSolicitud": "SOL-2025-001245",
          "estado": "APROBADO",
          "montoAprobado": 18500.00,
          "observaciones": "Cliente califica sin restricciones",
          "fechaRespuesta": "2025-07-10T10:25:00"
        }
       CONDICIONADO (exito)
       {
          "numeroSolicitud": "SOL-2025-001245",
          "estado": "CONDICIONADO",
          "montoAprobado": 12000,
          "tasa": 16.8,
          "observaciones": "Requiere codeudor",
          "condicionesAprobacion": [
            "Presentar codeudor con ingresos ≥ $1500",
            "Antigüedad laboral > 2 años",
            "No buró negativo"
          ]
        }
        CONDICIONADO (error)
       {
          "numeroSolicitud": "SOL-2025-001245",
          "estado": "CONDICIONADO",
          "montoAprobado": 12000,
          "tasa": 16.8,
          "observaciones": "Requiere codeudor",
        }
        NEGADO(exito)
        {
          "numeroSolicitud": "SOL-2025-001245",
          "estado": "NEGADO",
          "observaciones": "Ingresos bajos"
        }
        NEGADO(error)
        {
          "numeroSolicitud": "SOL-2025-001245",
          "estado": "NEGADO",
        }

       EN_PROCESO
        {
          "numeroSolicitud": "SOL-2025-001245",
          "estado": "EN_PROCESO",
          "observaciones": "La solicitud se encuentra en evaluación interna"
        }
       EN CONSOLA:
        warn: FinancieraCreditoApi.Services.RespuestaCreditoService[0]
        EN_PROCESO - Solicitud: SOL-2025-001245 | Reintento en 30 minutos ␦ 27/11/2025 14:36

   2. GET /api/creditos/respuesta/{numeroSolicitud}
   
      GET /api/creditos/respuesta/{SOL-2025-001245}
     
      EXITO
     {
        "mensaje": "Respuesta de crédito procesada correctamente",
        "data": {
          "id": 1,
          "idSolicitud": 2,
          "numeroSolicitud": "SOL-2025-001245",
          "estado": "APROBADO",
          "montoAprobado": 21500,
          "tasa": 13.9,
          "observaciones": "Cliente VIP - Aprobado sin restricciones",
          "fechaRespuesta": "2025-11-27T10:30:00",
          "jsonCompleto": "{\"NumeroSolicitud\":\"SOL-2025-001245\",\"Estado\":\"APROBADO\",\"MontoAprobado\":21500.00,\"Tasa\":13.90,\"Observaciones\":\"Cliente VIP - Aprobado sin restricciones\",\"FechaRespuesta\":\"2025-11-27T10:30:00\",\"Plazo\":60,\"CondicionesAprobacion\":null}",
          "fechaRegistro": "2025-11-27T00:07:37.653",
          "plazo": 60
        }
      }
   
   

   
