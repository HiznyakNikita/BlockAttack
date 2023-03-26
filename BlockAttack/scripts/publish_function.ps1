$function_id = $args[0]
$location = $args[1]
$project_location = $location + "\" + $function_id + "\src\" + $function_id 
Write-Output "Deploying " + $function_id + "from " + $project_location
dotnet lambda deploy-function $function_id --profile default --region us-east-1 --project-location $project_location
Write-Output "Done!"