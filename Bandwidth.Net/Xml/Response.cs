﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Bandwidth.Net.Xml
{
    /// <summary>
    /// Response class for Bandwidth XML
    /// </summary>
    [XmlRoot(Namespace = "")]
    public class Response: IXmlSerializable
    {
        private readonly List<IVerb> _list = new List<IVerb>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public Response()
        {
        }

        /// <summary>
        /// Constructor with verbs
        /// </summary>
        /// <param name="verbs">verbs to be added to response</param>
        public Response(params IVerb[] verbs)
        {
            _list.AddRange(verbs);
        }

        /// <summary>
        /// Add new verb to response
        /// </summary>
        /// <param name="verb">verb instance</param>
        public void Add(IVerb verb)
        {
            _list.Add(verb);
        }

        /// <summary>
        /// Returns XML for response
        /// </summary>
        /// <returns>Generated XML string</returns>
        public string ToXml()
        {
            using (var writer = new Utf8StringWriter{NewLine = "\n"})
            {
                Serializer.Serialize(writer, this);
                return writer.ToString();
            }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            throw new System.NotImplementedException();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            foreach (var verb in _list)
            {
                var serializer = new XmlSerializer(verb.GetType(), "");
                serializer.Serialize(writer, verb, ns);
            }
        }
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(Response), "");

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }
    }
}
