﻿using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace ConsoleApplication
{
    public class DGMLWriter
    {
        public class Graph
        {
            public Node[] Nodes;
            public Link[] Links;
        }

        public struct Node
        {
            [XmlAttribute]
            public string Id;
            [XmlAttribute]
            public string Label;
            [XmlAttribute]
            public string Bounds;
            [XmlAttribute]
            public string UseManualLocation;

            public Node(string id, string label, string bounds)
            {
                this.Id = id;
                this.Label = label;
                this.Bounds = bounds;
                this.UseManualLocation = "True";
            }
        }

        public struct Link
        {
            [XmlAttribute]
            public string Source;
            [XmlAttribute]
            public string Target;
            [XmlAttribute]
            public string Label;

            public Link(string source, string target, string label)
            {
                this.Source = source;
                this.Target = target;
                this.Label = label;
            }
        }

        public List<Node> Nodes { get; protected set; }
        public List<Link> Links { get; protected set; }

        public DGMLWriter()
        {
            Nodes = new List<Node>();
            Links = new List<Link>();
        }

        public void AddNode(Node n)
        {
            this.Nodes.Add(n);
        }

        public void AddLink(Link l)
        {
            this.Links.Add(l);
        }

        public void Serialize(string xmlpath)
        {
            Graph g = new Graph();
            g.Nodes = this.Nodes.ToArray();
            g.Links = this.Links.ToArray();

            XmlRootAttribute root = new XmlRootAttribute("DirectedGraph");
            root.Namespace = "http://schemas.microsoft.com/vs/2009/dgml";
            XmlSerializer serializer = new XmlSerializer(typeof(Graph), root);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter xmlWriter = XmlWriter.Create(xmlpath, settings);
            serializer.Serialize(xmlWriter, g);
        }

        public Graph Deserialize(string xmlpath)
        {
            XmlRootAttribute root = new XmlRootAttribute("DirectedGraph");
            root.Namespace = "http://schemas.microsoft.com/vs/2009/dgml";
            XmlSerializer serializer = new XmlSerializer(typeof(Graph), root);
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader xmlReader = XmlReader.Create(xmlpath, settings);
            return serializer.Deserialize(xmlReader) as Graph;
        }
    }
}