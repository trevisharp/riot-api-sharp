/* Date: 15/02/2024
 * Author: Leonardo Trevisan Silio
 */
using System;

namespace RiotApi.Exceptions;

public class InvalidRegionException : Exception
{
    public override string Message => "Invalid region.";
}