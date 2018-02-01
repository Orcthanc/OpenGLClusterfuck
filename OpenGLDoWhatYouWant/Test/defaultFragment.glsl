#version 430
// Things outputted to another shader
out vec4 FragColor;

// Things inputted by another shader (In this case defaultVertex.glsl)
in vec4 vertexColor;

// Selfexplanatory
void main(){
	FragColor = vertexColor;
}