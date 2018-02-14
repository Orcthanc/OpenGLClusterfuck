#version 430
// Things outputted to another shader
out vec4 FragColor;

// Things inputted by another shader (In this case defaultVertex.glsl)
in vec3 Normal;
in vec3 FragPos;
in vec2 texCoord;

// Things inputted via Code
uniform vec3 lightPos;
uniform vec3 lightColor;
uniform vec3 objectColor;
uniform sampler2D texture0;

// Selfexplanatory
void main(){
	// Ambient Lighting
	float ambientStrength = 0.2;
	vec3 ambient = ambientStrength * lightColor;

	// Diffuse Lighting
	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(lightPos - FragPos);
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diff * lightColor;

	// Combining everything
	FragColor = vec4((ambient + diffuse), 1) * texture(texture0, texCoord);

}