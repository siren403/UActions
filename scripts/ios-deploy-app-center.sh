echo "build start"

./scripts/fastlane-init.sh

cd fastlane

echo "--- Building"
bundle exec fastlane ios deploy_dev
echo "~~~ End Build"
