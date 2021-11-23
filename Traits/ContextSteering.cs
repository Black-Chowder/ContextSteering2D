using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spooky_Stealth.Utils;
using System.Diagnostics;

namespace Black_Magic
{
    //Class to handle attraction and repulsion nodes
    public class ContextVector
    {
        //Actual storage of position
        public float x { get; set; }
        public float y { get; set; }

        //Stores if vector is attraction vector or repulsion vector
        public bool isAttraction { get; set; }

        public bool exists { get; set; } = true;

        //Getter and setter for vector position
        public Vector2 pos
        {
            get
            {
                return new Vector2(x, y);
            }
            set
            {
                x = value.X;
                y = value.Y;
            }
        }

        //TODO: Add ability to create context vector with angle and weight

        public ContextVector(float x, float y, bool isAttraction = true)
        {
            this.x = x;
            this.y = y;
            this.isAttraction = isAttraction;
        }
        public ContextVector(Vector2 vector)
        {
            this.pos = vector;
        }
    }

    public class ContextSteering
    {
        //Constants
        public const int defaultResolution = 16;

        //Variables
        //Position of map
        public float x { get; set; }
        public float y { get; set; }

        //Map variables
        public int resolution { get; set; } = defaultResolution; // Number of weights context map will account for
        private double[] contextMap;

        //Getter for map
        public double[] GetContextMap()
        {
            return Realize();
        }

        //Variable to describe the desire / drive the context map has to reach a certian point
        public double drive { get; private set; } = 0d; 

        //Stores all context vectors applicable to this steering map
        private List<ContextVector> vectors;

        //Constructor
        public ContextSteering(float x = 0, float y = 0)
        {
            this.x = x;
            this.y = y;

            contextMap = new double[resolution];
            vectors = new List<ContextVector>();
        }

        //Fills Out The Context Map According To The Given Context Vectors
        public double[] Realize() //TODO: Account for when there are not valid attraction vectors, only repulsion vectors
        {
            //Instantiate maps
            double[] aMap = new double[resolution]; // Attraction Map
            double[] rMap = new double[resolution]; // Repulsion Map

            //Stores the distance between vectors and self
            double[] vectorDistances = new double[vectors.Count];

            //Find Max
            double maxDist = 0d;
            for (int i = 0; i < vectors.Count; i++)
            {
                //Store distance calculated in vector distances
                vectorDistances[i] = getDist(vectors[i].pos);

                //Calculate if this distance is max distance
                if (vectorDistances[i] > maxDist)
                    maxDist = vectorDistances[i];
            }

            //Calculate weight values for vectors
            for (int i = 0; i < vectors.Count; i++)
            {
                //Establish map being modified (attraction or repulsion)
                double[] modMap = vectors[i].isAttraction ? aMap : rMap;
                //Calculate angle between vector and context map position
                double vectorAtan = Math.Atan2(vectors[i].y - y, vectors[i].x - x);

                //Calculate weight values for each weight of map
                for (int j = 0; j < resolution; j++)
                {
                    //Calculate dot product
                    double dot = Dot(Math.PI * 2 / resolution * j, vectorAtan);

                    //Add applicable weight to map position
                    if (dot > 0) modMap[j] = Math.Max(modMap[j], dot * maxDist / vectorDistances[i]);
                }
            }

            //Combine Context Maps
            for (int i = 0; i < contextMap.Length; i++)
                contextMap[i] = aMap[i] > rMap[i] ? aMap[i] : 0;

            //Normalize context map so that values range between 1 and 0
            contextMap = Normalize(contextMap);

            //Return the context map generated
            return contextMap;
        }

        //Adds vector to be accounted for
        public void AddVector(ContextVector cv)
        {
            vectors.Add(cv);
        }

        //Removes vector to be accounted for
        public void DeleteVector(ContextVector cv)
        {
            vectors.Remove(cv);
        }

        //Clears vectors to be accounted for
        public void ClearVectors()
        {
            vectors.Clear();
        }

        //Debugging method to get attraction map
        public double[] GetAttractionMap() //TODO: Test and whatnot!!!
        {
            double[] map = new double[resolution];

            //Stores the distance between vectors and self
            double[] vectorDistances = new double[vectors.Count];

            //Find Max
            double maxDist = 0d;
            for (int i = 0; i < vectors.Count; i++)
            {
                //Store distance calculated in vector distances
                vectorDistances[i] = getDist(vectors[i].pos);

                //Calculate if this distance is max distance
                if (vectorDistances[i] > maxDist)
                    maxDist = vectorDistances[i];
            }

            //Calculate weight values for vectors
            for (int i = 0; i < vectors.Count; i++)
            {
                //Only calculate for attraction map
                if (!vectors[i].isAttraction) continue;

                //Calculate angle between vector and context map position
                double vectorAtan = Math.Atan2(vectors[i].y - y, vectors[i].x - x);

                //Calculate weight values for each weight of map
                for (int j = 0; j < resolution; j++)
                {
                    //Calculate dot product
                    double dot = Dot(Math.PI * 2 / resolution * j, vectorAtan);

                    //Add applicable weight to map position
                    if (dot > 0) map[j] = Math.Max(map[j], dot * maxDist / vectorDistances[i]);
                }
            }

            return Normalize(map);
        }

        //Debugging method to get repulsion map
        public double[] GetRepulsionMap()
        {
            double[] map = new double[resolution];

            //Stores the distance between vectors and self
            double[] vectorDistances = new double[vectors.Count];

            //Find Max
            double maxDist = 0d;
            for (int i = 0; i < vectors.Count; i++)
            {
                //Store distance calculated in vector distances
                vectorDistances[i] = getDist(vectors[i].pos);

                //Calculate if this distance is max distance
                if (vectorDistances[i] > maxDist)
                    maxDist = vectorDistances[i];
            }

            //Calculate weight values for vectors
            for (int i = 0; i < vectors.Count; i++)
            {
                //Only calculate for repulsion map
                if (vectors[i].isAttraction) continue;

                //Calculate angle between vector and context map position
                double vectorAtan = Math.Atan2(vectors[i].y - y, vectors[i].x - x);

                //Calculate weight values for each weight of map
                for (int j = 0; j < resolution; j++)
                {
                    //Calculate dot product
                    double dot = Dot(Math.PI * 2 / resolution * j, vectorAtan);

                    //Add applicable weight to map position
                    if (dot > 0) map[j] = Math.Max(map[j], dot * maxDist / vectorDistances[i]);
                }
            }

            return Normalize(map);
        }

        //Gets distance between self and given vector
        private double getDist(Vector2 given)
        {
            return Math.Sqrt((x - given.X) * (x - given.X) + (y - given.Y) * (y - given.Y));
        }

        //Gets dot product
        private static double Dot(double a1, double a2)
        {
            return Math.Cos(a1) * Math.Cos(a2) + Math.Sin(a1) * Math.Sin(a2);
        }

        //Normalizes an array of doubles so that all values range between 1 and 0
        private static double[] Normalize(double[] weights)
        {
            //Find longest value in weights array
            double longestValue = 0d;
            for (int i = 0; i < weights.Length; i++)
                if (longestValue < weights[i] && !double.IsInfinity(weights[i]))
                    longestValue = weights[i];

            //If all are 0, just return given map
            if (longestValue == 0) return weights;

            //Calculate Scalar Value
            double scalar = 1 / longestValue;

            //Scale context map
            for (int i = 0; i < weights.Length; i++)
                weights[i] *= scalar;

            return weights;
        }
    }
}
