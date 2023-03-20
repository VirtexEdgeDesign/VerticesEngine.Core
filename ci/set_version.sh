#!/bin/bash 
echo "Set Assembly Version"
VERSION=2.0.0
PROJECT_DIR="./src/shared"
CSPROJECT_DESKTOPGL="../desktop.gl/VerticesEngine.Core.Desktop.GL.csproj"
#DOX_PATH="./docs/Doxyfile"


echo "Setting Assembly and Nuget Pacakge versions to: "$VERSION.$@

# Set Desktop GL Version
sed -E -i "s/0.1.0/$VERSION.$@-alpha/g" "$PROJECT_DIR/$CSPROJECT_DESKTOPGL"

# Set Task Package Version
#sed -E -i "s/0.1.0/$VERSION.$@-alpha/g" "$PROJECT_DIR.Task/${CSPROJECT_TASK}"

# Set Doc Version
#sed -E -i "s/v1.0.0/v$VERSION.$@/g" ${DOX_PATH}

# Set Assembly Version
sed -E -i "s/2.0.0.999/$VERSION.$@/g" "$PROJECT_DIR/Properties/AssemblyInfo.cs"


echo "Done"