// ---------------------------------------------------------------------------
// <copyright file="OutlookUser.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------

//-----------------------------------------------------------------------
// <summary>Defines the OutlookUser class.</summary>
//-----------------------------------------------------------------------

namespace Microsoft.Exchange.WebServices.Autodiscover
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml;
    using Microsoft.Exchange.WebServices.Data;

    using ConverterDictionary = System.Collections.Generic.Dictionary<UserSettingName, System.Func<OutlookUser, string>>;
    using ConverterPair = System.Collections.Generic.KeyValuePair<UserSettingName, System.Func<OutlookUser, string>>;

    /// <summary>
    /// Represents the user Outlook configuration settings apply to.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class OutlookUser
    {
        /// <summary>
        /// Converters to translate Outlook user settings.
        /// Each entry maps to a lambda expression used to get the matching property from the OutlookUser instance. 
        /// </summary>
        private static LazyMember<ConverterDictionary> converterDictionary = new LazyMember<ConverterDictionary>(
            delegate()
            {
                var results = new ConverterDictionary();
                results.Add(UserSettingName.UserDisplayName,            u => u.displayName);
                results.Add(UserSettingName.UserDN,                     u => u.legacyDN);
                results.Add(UserSettingName.UserDeploymentId,           u => u.deploymentId);
                results.Add(UserSettingName.AutoDiscoverSMTPAddress,    u => u.autodiscoverAMTPAddress);
                return results;
            });

        #region Private fields
        private string displayName;
        private string legacyDN;
        private string deploymentId;
        private string autodiscoverAMTPAddress;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="OutlookUser"/> class.
        /// </summary>
        internal OutlookUser()
        {
        }

        /// <summary>
        /// Load from XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        internal void LoadFromXml(EwsXmlReader reader)
        {
            do
            {
                reader.Read();

                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case XmlElementNames.DisplayName:
                            this.displayName = reader.ReadElementValue();
                            break;
                        case XmlElementNames.LegacyDN:
                            this.legacyDN = reader.ReadElementValue();
                            break;
                        case XmlElementNames.DeploymentId:
                            this.deploymentId = reader.ReadElementValue();
                            break;
                        case XmlElementNames.AutoDiscoverSMTPAddress:
                            this.autodiscoverAMTPAddress = reader.ReadElementValue();
                            break;
                        default:
                            reader.SkipCurrentElement();
                            break;
                    }
                }
            }
            while (!reader.IsEndElement(XmlNamespace.NotSpecified, XmlElementNames.User));
        }

        /// <summary>
        /// Convert OutlookUser to GetUserSettings response.
        /// </summary>
        /// <param name="requestedSettings">The requested settings.</param>
        /// <param name="response">The response.</param>
        internal void ConvertToUserSettings(
            List<UserSettingName> requestedSettings,
            GetUserSettingsResponse response)
        {
            // In English: collect converters that are contained in the requested settings.
            var converterQuery = from converter in converterDictionary.Member 
                                 where requestedSettings.Contains(converter.Key) 
                                 select converter;

            foreach (ConverterPair kv in converterQuery)
            {
                string value = kv.Value(this);
                if (!string.IsNullOrEmpty(value))
                {
                    response.Settings[kv.Key] = value;
                }
            }
        }

        /// <summary>
        /// Gets the available user settings.
        /// </summary>
        /// <value>The available user settings.</value>
        internal static IEnumerable<UserSettingName> AvailableUserSettings
        {
            get { return converterDictionary.Member.Keys; }
        }
    }
}
