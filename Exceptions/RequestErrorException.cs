/* Date: 15/02/2024
 * Author: Leonardo Trevisan Silio
 */
using System;
using System.Net;

namespace RiotApi.Exceptions;

public class RequestErrorException(HttpStatusCode code, string message) : Exception
{
    public override string Message 
        => $"The request returns with {code} StatusCode and with content {message}.";
}