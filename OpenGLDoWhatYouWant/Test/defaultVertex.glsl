#version 430
layout (location = 0) in vec3 aPos;

// Things the shader outputs to another shader
out vec4 vertexColor;

// Things passed to the shader via code
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

// Selfexplanatory
void main(){
	gl_Position = projection * view * model * vec4(aPos, 1.0);
	vertexColor = vec4(1f, 1f, 0.14f, 1f);
}