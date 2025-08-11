# Appsettings Schema Validation

This project demonstrates how to integrate validation of your application settings into your application.

1. Static JSON schema validation of your appsettings.json file(s)
2. Runtime validation of the settings object(s).

## Static validation of appsettings.json file(s)

The static validation of the appsettings.json file is achieved using [JSON Schema](https://json-schema.org/). In our project the schema file is [MyServiceSettings.schema.json](./my-web-app/MyServiceSettings.schema.json).

### Visual Studio Code

For the validation to work in Visual Studio Code the schema needs to be defined in the (project) settings file in `.vscode/settings.json`.

```json
{
	"json.schemas": [
		{
			"fileMatch": [
				"/appsettings.json",
				"/appsettings.*.json"
			],
			"url": "./my-web-app/MyServiceSettings.schema.json"
		}
	]
}
```

### Visual Studio

For the validation to work in Visual Studio the `appsettings.json` file needs to have a `$schema` property that points to the schema file.

### JetBrains Rider

For the validation to work in JetBrains Rider, comparable to Visual Studio, the `appsettings.json` file needs to have a `$schema` property that points to the schema file.

## Runtime validation of settings object(s)

## Resources

- [https://app.quicktype.io/](https://app.quicktype.io/#l=schema): Generate JSON schemas from input data in various formats. 