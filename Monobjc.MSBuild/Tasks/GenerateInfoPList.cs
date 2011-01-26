﻿//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2011 - Laurent Etiemble
//
// Monobjc is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// any later version.
//
// Monobjc is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Monobjc.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Build.Utilities;
using Monobjc.MSBuild.Properties;
using Monobjc.Tools.Generators;
using Monobjc.Tools.Utilities;

namespace Monobjc.MSBuild.Tasks
{
    public class GenerateInfoPListTask : Task
    {
        private readonly InfoPListGenerator generator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateInfoPListTask"/> class.
        /// </summary>
        public GenerateInfoPListTask()
        {
            this.generator = new InfoPListGenerator();
        }

        /// <summary>
        /// Gets or sets the main assembly.
        /// </summary>
        /// <value>The main assembly.</value>
        public FileInfo MainAssembly { get; set; }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        public FileInfo Template { get; set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public String Icon { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public String Identifier { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public String Version { get; set; }

        /// <summary>
        /// Gets or sets the mininum required OS version.
        /// </summary>
        /// <value>The mininum required OS version.</value>
        public MacOSVersion MinRequiredOSVersion { get; set; }

        /// <summary>
        /// Gets or sets the main nib file.
        /// </summary>
        /// <value>The main nib file.</value>
        public String MainNibFile { get; set; }

        /// <summary>
        /// Gets or sets the principal class.
        /// </summary>
        /// <value>The principal class.</value>
        public String PrincipalClass { get; set; }

        /// <summary>
        /// Gets or sets the output dir.
        /// </summary>
        /// <value>The output dir.</value>
        public DirectoryInfo ToDirectory { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        public override bool Execute()
        {
            // Check that the mandatory information is set
            if (this.MainAssembly == null && this.ApplicationName == null)
            {
                this.Log.LogError(Resources.CannotComputeApplicationName);
                return false;
            }

            // Use the given template if any
            if (this.Template != null)
            {
                using (StreamReader reader = this.Template.OpenText())
                {
                    this.generator.Content = reader.ReadToEnd();
                }
            }

            // Extract information from the assembly if set
            if (this.MainAssembly != null)
            {
                Assembly assembly = Assembly.ReflectionOnlyLoadFrom(this.MainAssembly.ToString());
                AssemblyName assemblyName = assembly.GetName();
                this.ApplicationName = assemblyName.Name;
                this.Identifier = assembly.EntryPoint.DeclaringType.Namespace;
                this.Version = assemblyName.Version.ToString();
            }

            // Write the file
            String path = Path.Combine(this.ToDirectory.ToString(), "Info.plist");
            this.generator.WriteTo(path);

            return true;
        }
    }
}