# Source Generator Base :musical_keyboard:

Nevertheless, feel free to use or/and modify the code for Your own purpose. The IncrementalGeneratorBase and some utilities should be generic enough to be highly
reusable.

# How to use?

When referencing this project from other source generator, remember to add it as `PrivateAsset`

```
<ItemGroup>
	<ProjectReference Include="..\Shopway.SourceGenerator.Base\Shopway.SourceGenerator.Base.csproj" >
		<PrivateAssets>all</PrivateAssets>
	</ProjectReference>
</ItemGroup>
```