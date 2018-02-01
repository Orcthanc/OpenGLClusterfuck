#version 430
// Things outputted to another shader
out vec4 FragColor;

// Things inputted by another shader (In this case defaultVertex.glsl)
in vec4 vertexColor;
in vec3 Normal;

// Selfexplanatory
void main(){
	float Diff = max(dot(normalize(Normal), normalize(vec3(-1, 0, -0.2))), 0);

	FragColor = Diff * vertexColor;
}