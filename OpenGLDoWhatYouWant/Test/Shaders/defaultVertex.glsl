#version 430
// Collects data out of the currently bound vbo according to the data stored in the currently bound vao (See GL.VertexAttribPointer)
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 texCoords;
layout (location = 2) in vec3 aNormal;

// Things the shader outputs to another shader
out vec3 Normal;
out vec3 FragPos;
out vec2 texCoord;

// Things passed to the shader via code
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

// Selfexplanatory
void main(){
	FragPos = vec3(model * vec4(aPos, 1.0));
	gl_Position = projection * view * model * vec4(aPos, 1.0);
	Normal = mat3(transpose(inverse(model))) * aNormal;
	texCoord = texCoords;
}