// ---------------------------------------------------------------------------
// <copyright file="ExecuteDiagnosticMethodRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------

//-----------------------------------------------------------------------
// <summary>Defines the ExecuteDiagnosticMethodRequest class.</summary>
//-----------------------------------------------------------------------

namespace Microsoft.Exchange.WebServices.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Represents a ExecuteDiagnosticMethod request.
    /// </summary>
    internal sealed class ExecuteDiagnosticMethodRequest : MultiResponseServiceRequest<ExecuteDiagnosticMethodResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteDiagnosticMethodRequest"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        internal ExecuteDiagnosticMethodRequest(ExchangeService service)
            : base(service, ServiceErrorHandling.ThrowOnError)
        {
        }

        /// <summary>
        /// Gets the name of the XML element.
        /// </summary>
        /// <returns>XML element name,</returns>
        internal override string GetXmlElementName()
        {
            return XmlElementNames.ExecuteDiagnosticMethod;
        }

        /// <summary>
        /// Writes XML elements.
        /// </summary>
        /// <param name="writer">The writer.</param>
        internal override void WriteElementsToXml(EwsServiceXmlWriter writer)
        {
            writer.WriteElementValue(XmlNamespace.Messages, XmlElementNames.Verb, this.Verb);

            writer.WriteStartElement(XmlNamespace.Messages, XmlElementNames.Parameter);
            writer.WriteNode(this.Parameter);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Gets the name of the response XML element.
        /// </summary>
        /// <returns>XML element name,</returns>
        internal override string GetResponseXmlElementName()
        {
            return XmlElementNames.ExecuteDiagnosticMethodResponse;
        }

        /// <summary>
        /// Gets the request version.
        /// </summary>
        /// <returns>Earliest Exchange version in which this request is supported.</returns>
        internal override ExchangeVersion GetMinimumRequiredServerVersion()
        {
            // Set to 2007_SP1 because only test code will be using this method (it's marked internal.
            // If it were marked for 2010_SP1, test cases would have to create new ExchangeService instances 
            // when using this method for tests running under older versions.
            return ExchangeVersion.Exchange2007_SP1;
        }

        /// <summary>
        /// Gets or sets the verb of the method to execute.
        /// </summary>
        internal string Verb
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parameter to the executing method.
        /// </summary>
        internal XmlNode Parameter
        {
            get;
            set;
        }

        /// <summary>
        /// Creates the service response.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="responseIndex">Index of the response.</param>
        /// <returns>Service response.</returns>
        internal override ExecuteDiagnosticMethodResponse CreateServiceResponse(ExchangeService service, int responseIndex)
        {
            return new ExecuteDiagnosticMethodResponse(service);
        }

        /// <summary>
        /// Gets the name of the response message XML element.
        /// </summary>
        /// <returns>XML element name,</returns>
        internal override string GetResponseMessageXmlElementName()
        {
            return XmlElementNames.ExecuteDiagnosticMethodResponseMEssage;
        }

        /// <summary>
        /// Gets the expected response message count.
        /// </summary>
        /// <returns>Number of expected response messages.</returns>
        internal override int GetExpectedResponseMessageCount()
        {
            return 1;
        }
    }
}